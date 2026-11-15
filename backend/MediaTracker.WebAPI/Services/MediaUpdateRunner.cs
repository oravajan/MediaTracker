using System.Threading.Channels;
using MediaTracker.Application.Interfaces;

namespace MediaTracker.WebAPI.Services;

public class MediaUpdateRunner : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly Channel<bool> _requests = Channel.CreateUnbounded<bool>();
    private int _running;

    public MediaUpdateRunner(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public bool IsRunning => Volatile.Read(ref _running) == 1;

    public bool TryStart()
    {
        if (Interlocked.CompareExchange(ref _running, 1, 0) != 0)
            return false;

        _requests.Writer.TryWrite(true);
        return true;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var _ in _requests.Reader.ReadAllAsync(stoppingToken))
        {
            try
            {
                await RunAsync(stoppingToken);
            }
            finally
            {
                Volatile.Write(ref _running, 0);
            }
        }
    }

    private async Task RunAsync(CancellationToken stoppingToken)
    {
        try
        {
            List<Guid> ids;
            using (var scope = _scopeFactory.CreateScope())
            {
                ids = await scope.ServiceProvider.GetRequiredService<IMediaRepository>().GetAllIdsAsync();
            }

            Console.WriteLine($"Media update started ({ids.Count} items).");

            foreach (var id in ids)
            {
                if (stoppingToken.IsCancellationRequested)
                {
                    Console.WriteLine("Media update cancelled.");
                    return;
                }

                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    await scope.ServiceProvider.GetRequiredService<IMediaUpdater>().UpdateAsync(id);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Media update failed for {id}: {ex.Message}");
                }
            }

            Console.WriteLine("Media update finished.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Media update aborted: {ex.Message}");
        }
    }
}
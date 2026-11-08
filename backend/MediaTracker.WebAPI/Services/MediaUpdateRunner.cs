using MediaTracker.Application.Interfaces;

namespace MediaTracker.WebAPI.Services;

public class MediaUpdateRunner
{
    private readonly IServiceScopeFactory _scopeFactory;
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

        _ = Task.Run(RunAsync);
        return true;
    }

    private async Task RunAsync()
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
        finally
        {
            Volatile.Write(ref _running, 0);
        }
    }
}
using MediaTracker.Application.Interfaces;
using MediaTracker.Domain.Entities;

namespace MediaTracker.Application.Services;

public class NextMovieLinker : INextMovieLinker
{
    private readonly IMediaRepository _mediaRepository;
    private readonly ITmdbService _tmdbService;

    public NextMovieLinker(IMediaRepository mediaRepository, ITmdbService tmdbService)
    {
        _mediaRepository = mediaRepository;
        _tmdbService = tmdbService;
    }

    public async Task TryLinkAsync(Movie movie)
    {
        if (!movie.IsWatched || movie.NextMovieId.HasValue || !movie.TmdbId.HasValue)
            return;

        var next = await _tmdbService.GetNextMovieAsync(movie.TmdbId.Value);
        if (next is null)
            return;

        var sequel = await _mediaRepository.GetMovieByTmdbIdAsync(next.TmdbId);
        if (sequel is null)
        {
            sequel = new Movie(next.Title, null, null, false, next.TmdbId, next.ReleaseYear);
            await _mediaRepository.AddMovieAsync(sequel);
            await _mediaRepository.SaveChangesAsync();
        }
        else if (await WouldCreateCycleAsync(movie.Id, sequel.Id))
        {
            return;
        }

        movie.SetNextMovie(sequel.Id);
        await _mediaRepository.SaveChangesAsync();
    }

    public async Task<bool> WouldCreateCycleAsync(Guid movieId, Guid? nextMovieId)
    {
        var visited = new HashSet<Guid> { movieId };
        var currentId = nextMovieId;

        while (currentId is not null)
        {
            if (!visited.Add(currentId.Value))
                return true;

            var current = await _mediaRepository.GetMovieByIdAsync(currentId.Value);
            currentId = current?.NextMovieId;
        }

        return false;
    }
}
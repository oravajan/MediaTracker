using MediaTracker.Application.Interfaces;
using MediaTracker.Application.Mappers;
using MediaTracker.Domain.Entities;

namespace MediaTracker.Application.Services;

public class MediaUpdater : IMediaUpdater
{
    private readonly IMediaRepository _mediaRepository;
    private readonly ITmdbService _tmdbService;
    private readonly INextMovieLinker _nextMovieLinker;

    public MediaUpdater(IMediaRepository mediaRepository, ITmdbService tmdbService,
        INextMovieLinker nextMovieLinker)
    {
        _mediaRepository = mediaRepository;
        _tmdbService = tmdbService;
        _nextMovieLinker = nextMovieLinker;
    }

    public async Task UpdateAsync(Guid id)
    {
        var media = await _mediaRepository.GetByIdAsync(id);

        switch (media)
        {
            case Movie movie:
                await _nextMovieLinker.TryLinkAsync(movie);
                break;
            case TvShow tvShow:
                await UpdateTvShowAsync(tvShow);
                break;
        }
    }

    private async Task UpdateTvShowAsync(TvShow tvShow)
    {
        if (!tvShow.TmdbId.HasValue)
            return;

        var tmdbSeasons = await _tmdbService.GetReleasedSeasonsAsync(tvShow.TmdbId.Value);
        if (tmdbSeasons is null)
            return;

        foreach (var tmdbSeason in tmdbSeasons)
        {
            var local = tvShow.Seasons.FirstOrDefault(s => s.SeasonNumber == tmdbSeason.SeasonNumber);
            if (local is not null && local.Episodes.Count >= tmdbSeason.EpisodeCount)
                continue;

            var season = await _tmdbService.GetSeasonAsync(tvShow.TmdbId.Value, tmdbSeason.SeasonNumber);
            if (season is null)
                continue;

            tvShow.MergeSeasons(new[] { season.ToEntity() });
        }

        await _mediaRepository.SaveChangesAsync();
    }
}
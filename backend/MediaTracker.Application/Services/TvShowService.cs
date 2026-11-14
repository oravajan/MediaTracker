using MediaTracker.Application.DTOs;
using MediaTracker.Application.Exceptions;
using MediaTracker.Application.Interfaces;
using MediaTracker.Application.Mappers;
using MediaTracker.Domain.Entities;
using MediaTracker.Domain.Exceptions;

namespace MediaTracker.Application.Services;

public class TvShowService : ITvShowService
{
    private readonly IMediaRepository _mediaRepository;
    private readonly ITmdbService _tmdbService;

    public TvShowService(IMediaRepository mediaRepository, ITmdbService tmdbService)
    {
        _mediaRepository = mediaRepository;
        _tmdbService = tmdbService;
    }

    public async Task<TvShowDto> GetByIdAsync(Guid id)
    {
        var tvShow = await _mediaRepository.GetTvShowWithSeasonsAndEpisodesAsync(id);
        if (tvShow is null)
            throw new NotFoundException($"TV Show with id {id} was not found.");

        return tvShow.ToDto();
    }

    public async Task<TvShowDto> AddAsync(CreateTvShowDto dto)
    {
        var tvShow = dto.ToEntity();
        await _mediaRepository.AddTvShowAsync(tvShow);
        await _mediaRepository.SaveChangesAsync();

        if (tvShow.TmdbId.HasValue)
            await SyncWithTmdbAsync(tvShow.Id);

        return tvShow.ToDto();
    }

    public async Task<TvShowDto> UpdateAsync(Guid id, UpdateTvShowDto dto)
    {
        var tvShow = await _mediaRepository.GetTvShowByIdAsync(id);
        if (tvShow is null)
            throw new NotFoundException($"TV Show with id {id} was not found.");
        tvShow.Update(dto.Title, dto.UserRating, dto.TmdbId);
        await _mediaRepository.SaveChangesAsync();
        return tvShow.ToDto();
    }

    public async Task<SeasonDto> AddSeasonAsync(Guid tvShowId, CreateSeasonDto dto)
    {
        var tvShow = await _mediaRepository.GetTvShowWithSeasonsAsync(tvShowId);
        if (tvShow is null)
            throw new NotFoundException($"TV Show with id {tvShowId} was not found.");

        var season = dto.ToEntity();
        tvShow.AddSeason(season);
        await _mediaRepository.SaveChangesAsync();
        return season.ToDto();
    }

    public async Task<SeasonDto> UpdateSeasonAsync(Guid tvShowId, Guid seasonId, UpdateSeasonDto dto)
    {
        var tvShow = await _mediaRepository.GetTvShowWithSeasonsAsync(tvShowId);
        if (tvShow is null)
            throw new NotFoundException($"TV Show with id {tvShowId} was not found.");

        var season = tvShow.Seasons.FirstOrDefault(s => s.Id == seasonId);
        if (season is null)
            throw new NotFoundException($"Season with id {seasonId} was not found.");

        tvShow.UpdateSeason(season, dto.SeasonNumber, dto.ReleaseYear);
        await _mediaRepository.SaveChangesAsync();
        return season.ToDto();
    }

    public async Task DeleteSeasonAsync(Guid tvShowId, Guid seasonId)
    {
        var tvShow = await _mediaRepository.GetTvShowWithSeasonsAsync(tvShowId);
        if (tvShow is null)
            throw new NotFoundException($"TV Show with id {tvShowId} was not found.");

        var season = tvShow.Seasons.FirstOrDefault(s => s.Id == seasonId);
        if (season is null)
            throw new NotFoundException($"Season with id {seasonId} was not found.");

        tvShow.RemoveSeason(season);
        await _mediaRepository.SaveChangesAsync();
    }

    public async Task<SeasonDto> MarkWatchedSeasonAsync(Guid tvShowId, Guid seasonId, MarkWatchedSeasonDto dto)
    {
        var season = await GetSeasonWithEpisodesAsync(tvShowId, seasonId);

        season.MarkWatched(dto.IsWatched);
        await _mediaRepository.SaveChangesAsync();
        return season.ToDto();
    }

    public async Task<EpisodeDto> AddEpisodeAsync(Guid tvShowId, Guid seasonId, CreateEpisodeDto dto)
    {
        var season = await GetSeasonWithEpisodesAsync(tvShowId, seasonId);

        var episode = dto.ToEntity();
        season.AddEpisode(episode);
        await _mediaRepository.SaveChangesAsync();
        return episode.ToDto();
    }

    public async Task<EpisodeDto> UpdateEpisodeAsync(Guid tvShowId, Guid seasonId, Guid episodeId,
        UpdateEpisodeDto dto)
    {
        var season = await GetSeasonWithEpisodesAsync(tvShowId, seasonId);
        var episode = GetEpisode(season, episodeId);

        season.UpdateEpisode(episode, dto.EpisodeNumber, dto.Title, dto.IsWatched);
        await _mediaRepository.SaveChangesAsync();
        return episode.ToDto();
    }

    public async Task DeleteEpisodeAsync(Guid tvShowId, Guid seasonId, Guid episodeId)
    {
        var season = await GetSeasonWithEpisodesAsync(tvShowId, seasonId);
        var episode = GetEpisode(season, episodeId);

        season.RemoveEpisode(episode);
        await _mediaRepository.SaveChangesAsync();
    }

    public async Task<EpisodeDto> MarkWatchedEpisodeAsync(Guid tvShowId, Guid seasonId, Guid episodeId,
        MarkWatchedEpisodeDto dto)
    {
        var season = await GetSeasonWithEpisodesAsync(tvShowId, seasonId);
        var episode = GetEpisode(season, episodeId);

        episode.MarkWatched(dto.IsWatched);
        await _mediaRepository.SaveChangesAsync();
        return episode.ToDto();
    }

    public async Task SyncWithTmdbAsync(Guid tvShowId)
    {
        var tvShow = await _mediaRepository.GetTvShowWithSeasonsAsync(tvShowId);
        if (tvShow is null)
            throw new NotFoundException($"TV Show with id {tvShowId} was not found.");

        if (!tvShow.TmdbId.HasValue)
            throw new DomainException("TV Show is not linked to TMDB.");

        var tmdbData = await _tmdbService.GetTvShowSeasonsWithEpisodesAsync(tvShow.TmdbId.Value);
        if (tmdbData is null)
            throw new NotFoundException($"TV Show with TMDB id {tvShow.TmdbId} was not found.");

        tvShow.ReplaceSeasons(tmdbData.Seasons.Select(s => s.ToEntity()));
        await _mediaRepository.SaveChangesAsync();
    }

    private async Task<Season> GetSeasonWithEpisodesAsync(Guid tvShowId, Guid seasonId)
    {
        var season = await _mediaRepository.GetSeasonWithEpisodesAsync(tvShowId, seasonId);
        if (season is null)
            throw new NotFoundException($"Season with id {seasonId} was not found.");

        return season;
    }

    private static Episode GetEpisode(Season season, Guid episodeId)
    {
        var episode = season.Episodes.FirstOrDefault(e => e.Id == episodeId);
        if (episode is null)
            throw new NotFoundException($"Episode with id {episodeId} was not found.");

        return episode;
    }
}
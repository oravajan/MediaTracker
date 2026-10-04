using MediaTracker.Application.DTOs;
using MediaTracker.Application.Exceptions;
using MediaTracker.Application.Interfaces;
using MediaTracker.Application.Mappers;
using MediaTracker.Domain.Entities;

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

    public async Task<SeasonDto> UpdateSeasonAsync(Guid seasonId, UpdateSeasonDto dto)
    {
        var season = await _mediaRepository.GetSeasonByIdAsync(seasonId);
        if (season is null)
            throw new NotFoundException($"Season with id {seasonId} was not found.");

        season.Update(dto.SeasonNumber, dto.ReleaseYear);
        await _mediaRepository.SaveChangesAsync();
        return season.ToDto();
    }

    public async Task DeleteSeasonAsync(Guid seasonId)
    {
        await _mediaRepository.DeleteSeasonAsync(seasonId);
        await _mediaRepository.SaveChangesAsync();
    }

    public async Task<EpisodeDto> AddEpisodeAsync(Guid seasonId, CreateEpisodeDto dto)
    {
        var season = await _mediaRepository.GetSeasonWithEpisodesAsync(seasonId);
        if (season is null)
            throw new NotFoundException($"Season with id {seasonId} was not found.");

        var episode = dto.ToEntity();
        season.AddEpisode(episode);
        await _mediaRepository.SaveChangesAsync();
        return episode.ToDto();
    }

    public async Task<EpisodeDto> UpdateEpisodeAsync(Guid episodeId, UpdateEpisodeDto dto)
    {
        var episode = await _mediaRepository.GetEpisodeByIdAsync(episodeId);
        if (episode is null)
            throw new NotFoundException($"Episode with id {episodeId} was not found.");

        episode.Update(dto.EpisodeNumber, dto.Title, dto.IsWatched);
        await _mediaRepository.SaveChangesAsync();
        return episode.ToDto();
    }

    public async Task DeleteEpisodeAsync(Guid episodeId)
    {
        await _mediaRepository.DeleteEpisodeAsync(episodeId);
        await _mediaRepository.SaveChangesAsync();
    }

    public async Task<EpisodeDto> MarkWatchedEpisodeAsync(Guid episodeId, MarkWatchedEpisodeDto dto)
    {
        var episode = await _mediaRepository.GetEpisodeByIdAsync(episodeId);
        if (episode is null)
            throw new NotFoundException($"Episode with id {episodeId} was not found.");

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
            throw new InvalidOperationException("TV Show is not linked to TMDB.");

        var tmdbData = await _tmdbService.GetTvShowSeasonsAsync(tvShow.TmdbId.Value);
        if (tmdbData is null)
            throw new NotFoundException($"TV Show with TMDB id {tvShow.TmdbId} was not found.");

        var seasons = tmdbData.Seasons.Select(s =>
        {
            var season = new Season(s.SeasonNumber, s.ReleaseYear);

            foreach (var episode in s.Episodes)
                season.AddEpisode(new Episode(episode.EpisodeNumber, episode.Title, false));
            
            return season;
        }).ToList();

        tvShow.ReplaceSeasons(seasons);
        await _mediaRepository.SaveChangesAsync();
    }
}
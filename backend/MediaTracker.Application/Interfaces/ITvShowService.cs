using MediaTracker.Application.DTOs;

namespace MediaTracker.Application.Interfaces;

public interface ITvShowService
{
    Task<TvShowDto> GetByIdAsync(Guid id);
    Task<TvShowDto> AddAsync(CreateTvShowDto dto);
    Task<TvShowDto> UpdateAsync(Guid id, UpdateTvShowDto dto);

    Task<SeasonDto> AddSeasonAsync(Guid tvShowId, CreateSeasonDto dto);
    Task<SeasonDto> UpdateSeasonAsync(Guid tvShowId, Guid seasonId, UpdateSeasonDto dto);
    Task DeleteSeasonAsync(Guid seasonId);
    Task<SeasonDto> MarkWatchedSeasonAsync(Guid seasonId, MarkWatchedSeasonDto dto);

    Task<EpisodeDto> AddEpisodeAsync(Guid seasonId, CreateEpisodeDto dto);
    Task<EpisodeDto> UpdateEpisodeAsync(Guid seasonId, Guid episodeId, UpdateEpisodeDto dto);
    Task DeleteEpisodeAsync(Guid episodeId);
    Task<EpisodeDto> MarkWatchedEpisodeAsync(Guid episodeId, MarkWatchedEpisodeDto dto);
    
    Task SyncWithTmdbAsync(Guid tvShowId);
}
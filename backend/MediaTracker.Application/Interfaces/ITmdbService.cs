using MediaTracker.Application.DTOs;

namespace MediaTracker.Application.Interfaces;

public interface ITmdbService
{
    bool IsConfigured { get; }
    Task<IEnumerable<TmdbMovieSearchDto>> SearchMoviesAsync(string query);
    Task<IEnumerable<TmdbTvShowSearchDto>> SearchTvShowsAsync(string query);
    Task<TmdbTvShowSeasonsDto?> GetTvShowSeasonsWithEpisodesAsync(int tmdbId);
    Task<TmdbMovieSearchDto?> GetNextMovieAsync(int tmdbId);
    Task<List<TmdbSeasonSummaryDto>?> GetReleasedSeasonsAsync(int tmdbId);
    Task<TmdbSeasonInfoDto?> GetSeasonWithEpisodesAsync(int tmdbId, int seasonNumber);
}
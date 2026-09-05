using MediaTracker.Application.DTOs;

namespace MediaTracker.Application.Interfaces;

public interface ITmdbService
{
    bool IsConfigured { get; }
    Task<IEnumerable<TmdbMovieSearchDto>> SearchMoviesAsync(string query);
    Task<IEnumerable<TmdbTvShowSearchDto>> SearchTvShowsAsync(string query);
    Task<TmdbTvShowSeasonsDto?> GetTvShowSeasonsAsync(int tmdbId);
}
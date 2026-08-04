using MediaTracker.Domain.Entities;

namespace MediaTracker.Application.Interfaces;

public interface IMediaRepository
{
    Task<IEnumerable<Media>> GetAllAsync();
    Task<Media?> GetByIdAsync(Guid id);
    Task DeleteAsync(Guid id);

    Task<Movie?> GetMovieByIdAsync(Guid id);
    Task AddMovieAsync(Movie movie);

    Task<TvShow?> GetTvShowByIdAsync(Guid id);
    Task<TvShow?> GetTvShowWithSeasonsAsync(Guid id);
    Task<TvShow?> GetTvShowWithSeasonsAndEpisodesAsync(Guid id);
    Task AddTvShowAsync(TvShow tvShow);

    Task<Season?> GetSeasonByIdAsync(Guid seasonId);
    Task DeleteSeasonAsync(Guid seasonId);
    Task<Episode?> GetEpisodeByIdAsync(Guid seasonId);
    Task DeleteEpisodeAsync(Guid episodeId);

    Task SaveChangesAsync();
}
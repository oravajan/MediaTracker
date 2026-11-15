using MediaTracker.Domain.Entities;

namespace MediaTracker.Application.Interfaces;

public interface IMediaRepository
{
    Task<IEnumerable<Media>> GetAllAsync();
    Task<List<Guid>> GetAllIdsAsync();
    Task<Media?> GetByIdAsync(Guid id);
    Task<Media?> FindAsync(Guid id);
    void Remove(Media media);

    Task<Movie?> GetMovieByIdAsync(Guid id);
    Task<Movie?> GetMovieByTmdbIdAsync(int tmdbId);
    Task AddMovieAsync(Movie movie);

    Task<TvShow?> GetTvShowWithSeasonsAsync(Guid id);
    Task<TvShow?> GetTvShowWithSeasonsAndEpisodesAsync(Guid id);
    Task AddTvShowAsync(TvShow tvShow);

    Task<Season?> GetSeasonWithEpisodesAsync(Guid tvShowId, Guid seasonId);

    Task SaveChangesAsync();
}
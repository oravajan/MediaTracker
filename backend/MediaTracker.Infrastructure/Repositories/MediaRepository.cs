using MediaTracker.Application.Interfaces;
using MediaTracker.Domain.Entities;
using MediaTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MediaTracker.Infrastructure.Repositories;

public class MediaRepository : IMediaRepository
{
    private readonly AppDbContext _context;

    public MediaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Media>> GetAllAsync()
    {
        return await _context.Media
            .Include(m => (m as TvShow)!.Seasons)
            .ThenInclude(s => s.Episodes)
            .AsSplitQuery()
            .ToListAsync();
    }

    public async Task<Media?> GetByIdAsync(Guid id)
    {
        return await _context.Media.FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task DeleteAsync(Guid id)
    {
        var media = await GetByIdAsync(id);
        if (media != null)
            _context.Media.Remove(media);
    }

    public async Task<Movie?> GetMovieByIdAsync(Guid id)
    {
        return await _context.Media
            .OfType<Movie>()
            .Include(m => m.NextMovie)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task AddMovieAsync(Movie movie)
    {
        await _context.Media.AddAsync(movie);
    }

    public async Task<TvShow?> GetTvShowByIdAsync(Guid id)
    {
        return await _context.Media
            .OfType<TvShow>()
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<TvShow?> GetTvShowWithSeasonsAsync(Guid id)
    {
        return await _context.Media
            .OfType<TvShow>()
            .Include(t => t.Seasons)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<TvShow?> GetTvShowWithSeasonsAndEpisodesAsync(Guid id)
    {
        return await _context.Media
            .OfType<TvShow>()
            .Include(t => t.Seasons)
            .ThenInclude(s => s.Episodes)
            .AsSplitQuery()
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task AddTvShowAsync(TvShow tvShow)
    {
        await _context.Media.AddAsync(tvShow);
    }

    public async Task<Season?> GetSeasonByIdAsync(Guid seasonId)
    {
        return await _context.Season.FirstOrDefaultAsync(s => s.Id == seasonId);
    }

    public async Task DeleteSeasonAsync(Guid seasonId)
    {
        var season = await GetSeasonByIdAsync(seasonId);
        if (season != null)
            _context.Season.Remove(season);
    }

    public async Task<Episode?> GetEpisodeByIdAsync(Guid seasonId)
    {
        return await _context.Episode.FirstOrDefaultAsync(e => e.Id == seasonId);
    }

    public async Task DeleteEpisodeAsync(Guid episodeId)
    {
        var episode = await GetEpisodeByIdAsync(episodeId);
        if (episode != null)
            _context.Episode.Remove(episode);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
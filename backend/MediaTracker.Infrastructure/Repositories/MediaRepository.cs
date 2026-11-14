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
    
    public async Task<List<Guid>> GetAllIdsAsync()
    {
        return await _context.Media.Select(m => m.Id).ToListAsync();
    }

    public async Task<Media?> GetByIdAsync(Guid id)
    {
        return await _context.Media
            .Include(m => (m as TvShow)!.Seasons)
            .ThenInclude(s => s.Episodes)
            .AsSplitQuery()
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task DeleteAsync(Guid id)
    {
        var media = await _context.Media.FindAsync(id);
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
    
    public async Task<Movie?> GetMovieByTmdbIdAsync(int tmdbId)
    {
        return await _context.Media
            .OfType<Movie>()
            .Include(m => m.NextMovie)
            .FirstOrDefaultAsync(m => m.TmdbId == tmdbId);
    }

    public async Task AddMovieAsync(Movie movie)
    {
        await _context.Media.AddAsync(movie);
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

    public async Task<Season?> GetSeasonWithEpisodesAsync(Guid tvShowId, Guid seasonId)
    {
        return await _context.Media
            .OfType<TvShow>()
            .Where(t => t.Id == tvShowId)
            .SelectMany(t => t.Seasons)
            .Include(s => s.Episodes)
            .AsSplitQuery()
            .FirstOrDefaultAsync(s => s.Id == seasonId);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
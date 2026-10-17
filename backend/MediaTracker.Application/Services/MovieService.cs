using MediaTracker.Application.DTOs;
using MediaTracker.Application.Exceptions;
using MediaTracker.Application.Interfaces;
using MediaTracker.Application.Mappers;
using MediaTracker.Domain.Exceptions;

namespace MediaTracker.Application.Services;

public class MovieService : IMovieService
{
    private readonly IMediaRepository _mediaRepository;
    private readonly INextMovieLinker _nextMovieLinker;

    public MovieService(IMediaRepository mediaRepository, INextMovieLinker nextMovieLinker)
    {
        _mediaRepository = mediaRepository;
        _nextMovieLinker = nextMovieLinker;
    }

    public async Task<MovieDto> GetByIdAsync(Guid id)
    {
        var movie = await _mediaRepository.GetMovieByIdAsync(id);
        if (movie is null)
            throw new NotFoundException($"Movie with id {id} was not found.");

        return movie.ToDto();
    }

    public async Task<MovieDto> AddAsync(CreateMovieDto dto)
    {
        var movie = dto.ToEntity();
        await _mediaRepository.AddMovieAsync(movie);
        await _mediaRepository.SaveChangesAsync();
        
        if (!movie.NextMovieId.HasValue) 
            return movie.ToDto();
        
        return await GetByIdAsync(movie.Id);
    }

    public async Task<MovieDto> UpdateAsync(Guid id, UpdateMovieDto dto)
    {
        var movie = await _mediaRepository.GetMovieByIdAsync(id);
        if (movie is null)
            throw new NotFoundException($"Movie with id {id} was not found.");
        
        if (await _nextMovieLinker.WouldCreateCycleAsync(id, dto.NextMovieId))
            throw new DomainException("Setting this movie as next would create a circular reference.");
        
        var wasWatched = movie.IsWatched;
        movie.Update(dto.Title, dto.UserRating, dto.NextMovieId, dto.IsWatched, dto.TmdbId, dto.ReleaseYear);
        await _mediaRepository.SaveChangesAsync();
        
        if (!wasWatched && movie.IsWatched)
            await _nextMovieLinker.TryLinkAsync(movie);
        
        return await GetByIdAsync(id);
    }
}
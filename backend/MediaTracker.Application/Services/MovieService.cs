using MediaTracker.Application.DTOs;
using MediaTracker.Application.Exceptions;
using MediaTracker.Application.Interfaces;
using MediaTracker.Application.Mappers;
using MediaTracker.Domain.Exceptions;

namespace MediaTracker.Application.Services;

public class MovieService : IMovieService
{
    private readonly IMediaRepository _mediaRepository;

    public MovieService(IMediaRepository mediaRepository)
    {
        _mediaRepository = mediaRepository;
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
        
        await ValidateNoCircularReferenceAsync(id, dto.NextMovieId);
        
        movie.Update(dto.Title, dto.UserRating, dto.NextMovieId, dto.IsWatched);
        await _mediaRepository.SaveChangesAsync();
        return await GetByIdAsync(id);
    }

    public async Task<MovieDto> MarkWatchedAsync(Guid movieId, MarkWatchedMovieDto dto)
    {
        var movie = await _mediaRepository.GetMovieByIdAsync(movieId);
        if (movie is null)
            throw new NotFoundException($"Movie with id {movieId} was not found.");

        movie.MarkWatched(dto.IsWatched);
        await _mediaRepository.SaveChangesAsync();
        return movie.ToDto();
    }
    
    private async Task ValidateNoCircularReferenceAsync(Guid movieId, Guid? nextMovieId)
    {
        if (nextMovieId is null) return;

        var visited = new HashSet<Guid> { movieId };
        var currentId = nextMovieId;

        while (currentId is not null)
        {
            if (!visited.Add(currentId.Value))
                throw new DomainException("Setting this movie as next would create a circular reference.");

            var current = await _mediaRepository.GetMovieByIdAsync(currentId.Value);
            currentId = current?.NextMovieId;
        }
    }
}
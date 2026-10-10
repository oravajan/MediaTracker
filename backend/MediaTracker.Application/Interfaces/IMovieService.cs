using MediaTracker.Application.DTOs;

namespace MediaTracker.Application.Interfaces;

public interface IMovieService
{
    Task<MovieDto> GetByIdAsync(Guid id);
    Task<MovieDto> AddAsync(CreateMovieDto dto);
    Task<MovieDto> UpdateAsync(Guid id, UpdateMovieDto dto);
}
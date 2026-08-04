using MediaTracker.Application.DTOs;
using MediaTracker.Domain.Entities;

namespace MediaTracker.Application.Mappers;

public static class MovieMapper
{
    public static Movie ToEntity(this CreateMovieDto dto)
    {
        return new Movie(Guid.Empty, dto.Title, dto.UserRating, dto.NextMovieId, null);
    }

    public static MovieDto ToDto(this Movie movie)
    {
        return new MovieDto(movie.Id, movie.Title, movie.UserRating, movie.NextMovieId, movie.NextMovie?.Title);
    }
}
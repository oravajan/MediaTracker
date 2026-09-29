using MediaTracker.Application.DTOs;
using MediaTracker.Domain.Entities;

namespace MediaTracker.Application.Mappers;

public static class MovieMapper
{
    public static Movie ToEntity(this CreateMovieDto dto)
    {
        return new Movie(dto.Title, dto.UserRating, dto.NextMovieId, false, dto.TmdbId, dto.ReleaseYear);
    }

    public static MovieDto ToDto(this Movie movie)
    {
        return new MovieDto(movie.Id, movie.Title, movie.UserRating, movie.NextMovieId, movie.IsWatched, movie.TmdbId,
            movie.ReleaseYear);
    }
}
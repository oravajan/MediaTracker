namespace MediaTracker.Application.DTOs;

public record MovieDto(
    Guid Id,
    string Title,
    int? UserRating,
    Guid? NextMovieId,
    bool IsWatched,
    int? TmdbId);

public record CreateMovieDto(string Title, int? UserRating, Guid? NextMovieId, int? TmdbId);

public record UpdateMovieDto(string Title, int? UserRating, Guid? NextMovieId, bool IsWatched, int? TmdbId);

public record MarkWatchedMovieDto(bool IsWatched);
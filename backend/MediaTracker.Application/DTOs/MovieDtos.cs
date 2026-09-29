namespace MediaTracker.Application.DTOs;

public record MovieDto(
    Guid Id,
    string Title,
    int? UserRating,
    Guid? NextMovieId,
    bool IsWatched,
    int? TmdbId,
    int? ReleaseYear);

public record CreateMovieDto(string Title, int? UserRating, Guid? NextMovieId, int? TmdbId, int? ReleaseYear);

public record UpdateMovieDto(
    string Title,
    int? UserRating,
    Guid? NextMovieId,
    bool IsWatched,
    int? TmdbId,
    int? ReleaseYear);

public record MarkWatchedMovieDto(bool IsWatched);
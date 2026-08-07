namespace MediaTracker.Application.DTOs;

public record MovieDto(
    Guid Id,
    string Title,
    int? UserRating,
    Guid? NextMovieId,
    bool IsWatched);

public record CreateMovieDto(string Title, int? UserRating, Guid? NextMovieId);

public record UpdateMovieDto(string Title, int? UserRating, Guid? NextMovieId, bool IsWatched);

public record MarkWatchedMovieDto(bool IsWatched);
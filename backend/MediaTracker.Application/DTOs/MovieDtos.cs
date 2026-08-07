namespace MediaTracker.Application.DTOs;

public record MovieDto(
    Guid Id,
    string Title,
    int? UserRating,
    Guid? NextMovieId,
    bool IsWatched) : MediaDto(Id, Title, UserRating);

public record CreateMovieDto(string Title, int? UserRating, Guid? NextMovieId)
    : CreateMediaDto(Title, UserRating);

public record UpdateMovieDto(string Title, int? UserRating, Guid? NextMovieId, bool IsWatched)
    : UpdateMediaDto(Title, UserRating);

public record MarkWatchedMovieDto(bool IsWatched);
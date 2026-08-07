namespace MediaTracker.Application.DTOs;

public abstract record MediaDto(Guid Id, string Title, int? UserRating);

public abstract record CreateMediaDto(string Title, int? UserRating);

public abstract record UpdateMediaDto(string Title, int? UserRating);

public record MediaSummaryDto(
    Guid Id,
    string Title,
    string Type,
    int? UserRating,
    bool IsWatched,
    int? WatchedEpisodeCount,
    int? TotalEpisodeCount)
    : MediaDto(Id, Title, UserRating);
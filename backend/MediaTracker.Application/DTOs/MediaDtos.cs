namespace MediaTracker.Application.DTOs;

public record MediaSummaryDto(
    Guid Id,
    string Title,
    string Type,
    int? UserRating,
    bool IsWatched,
    int? WatchedEpisodeCount,
    int? TotalEpisodeCount);
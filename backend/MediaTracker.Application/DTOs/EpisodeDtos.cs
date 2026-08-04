namespace MediaTracker.Application.DTOs;

public record EpisodeDto(Guid Id, int EpisodeNumber, string? Title, bool IsWatched);

public record CreateEpisodeDto(int EpisodeNumber, string? Title, bool IsWatched);

public record UpdateEpisodeDto(int EpisodeNumber, string? Title, bool IsWatched);

public record MarkWatchedEpisodeDto(bool IsWatched);
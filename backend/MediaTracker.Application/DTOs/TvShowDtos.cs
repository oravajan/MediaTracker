namespace MediaTracker.Application.DTOs;

public record TvShowDto(Guid Id, string Title, int? UserRating, List<SeasonDto> Seasons, int? TmdbId, int? ReleaseYear);

public record CreateTvShowDto(string Title, int? UserRating, int? TmdbId);

public record UpdateTvShowDto(string Title, int? UserRating, int? TmdbId);
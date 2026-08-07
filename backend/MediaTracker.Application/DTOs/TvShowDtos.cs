namespace MediaTracker.Application.DTOs;

public record TvShowDto(Guid Id, string Title, int? UserRating, List<SeasonDto> Seasons);

public record CreateTvShowDto(string Title, int? UserRating);

public record UpdateTvShowDto(string Title, int? UserRating);
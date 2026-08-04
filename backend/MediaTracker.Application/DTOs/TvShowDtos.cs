namespace MediaTracker.Application.DTOs;

public record TvShowDto(Guid Id, string Title, int? UserRating, List<SeasonDto> Seasons)
    : MediaDto(Id, Title, UserRating);

public record CreateTvShowDto(string Title, int? UserRating) : CreateMediaDto(Title, UserRating);

public record UpdateTvShowDto(string Title, int? UserRating) : UpdateMediaDto(Title, UserRating);
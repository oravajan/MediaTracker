namespace MediaTracker.Application.DTOs;

public record SeasonDto(Guid Id, int SeasonNumber, List<EpisodeDto> Episodes, int? ReleaseYear);

public record CreateSeasonDto(int SeasonNumber, int? ReleaseYear);

public record UpdateSeasonDto(int SeasonNumber, int? ReleaseYear);
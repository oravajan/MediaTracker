namespace MediaTracker.Application.DTOs;

public record SeasonDto(Guid Id, int SeasonNumber, List<EpisodeDto> Episodes);

public record CreateSeasonDto(int SeasonNumber);

public record UpdateSeasonDto(int SeasonNumber);
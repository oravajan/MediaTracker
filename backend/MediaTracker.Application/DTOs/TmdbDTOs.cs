namespace MediaTracker.Application.DTOs;

public record TmdbMovieSearchDto(
    int TmdbId,
    string Title,
    int? ReleaseYear);

public record TmdbTvShowSearchDto(
    int TmdbId,
    string Name);

public record TmdbTvShowSeasonsDto(
    int TmdbId,
    string Name,
    List<TmdbSeasonInfoDto> Seasons);

public record TmdbSeasonInfoDto(
    int SeasonNumber,
    int EpisodeCount,
    int? ReleaseYear);
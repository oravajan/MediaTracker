namespace MediaTracker.Application.DTOs;

public record TmdbMovieSearchDto(
    int TmdbId,
    string Title,
    int? ReleaseYear);

public record TmdbTvShowSearchDto(
    int TmdbId,
    string Title);

public record TmdbTvShowSeasonsDto(
    int TmdbId,
    string Title,
    List<TmdbSeasonInfoDto> Seasons);

public record TmdbSeasonInfoDto(
    int SeasonNumber,
    int? ReleaseYear,
    List<TmdbEpisodeInfoDto> Episodes);

public record TmdbEpisodeInfoDto(
    int EpisodeNumber,
    string? Title);
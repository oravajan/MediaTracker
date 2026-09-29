using System.Text.Json.Serialization;

namespace MediaTracker.Infrastructure.ExternalServices.Tmdb.Models;

public record TmdbSearchResponse<T>(
    [property: JsonPropertyName("results")] List<T> Results);

public record TmdbMovieResult(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("title")] string? Title,
    [property: JsonPropertyName("release_date")] string? ReleaseDate);

public record TmdbTvShowResult(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("name")] string? Name,
    [property: JsonPropertyName("first_air_date")] string? FirstAirDate);

public record TmdbTvShowDetail(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("seasons")] List<TmdbSeasonDetail> Seasons);

public record TmdbSeasonDetail(
    [property: JsonPropertyName("season_number")] int SeasonNumber,
    [property: JsonPropertyName("air_date")] string? AirDate,
    [property: JsonPropertyName("episodes")] List<TmdbEpisodeResult> Episodes);
    
public record TmdbEpisodeResult(
    [property: JsonPropertyName("episode_number")] int EpisodeNumber,
    [property: JsonPropertyName("name")] string? Name,
    [property: JsonPropertyName("air_date")] string? AirDate);
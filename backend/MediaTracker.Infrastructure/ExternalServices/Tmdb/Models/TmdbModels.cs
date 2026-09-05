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
    [property: JsonPropertyName("name")] string? Name);

public record TmdbTvShowDetail(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("seasons")] List<TmdbSeasonResult> Seasons);

public record TmdbSeasonResult(
    [property: JsonPropertyName("season_number")] int SeasonNumber,
    [property: JsonPropertyName("episode_count")] int EpisodeCount,
    [property: JsonPropertyName("air_date")] string? AirDate);
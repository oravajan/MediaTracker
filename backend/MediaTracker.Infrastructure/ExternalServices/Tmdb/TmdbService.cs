using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using MediaTracker.Application.DTOs;
using MediaTracker.Application.Interfaces;
using MediaTracker.Infrastructure.ExternalServices.Tmdb.Models;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace MediaTracker.Infrastructure.ExternalServices.Tmdb;

public class TmdbService : ITmdbService
{
    private readonly HttpClient _httpClient;
    private readonly bool _isConfigured;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public bool IsConfigured => _isConfigured;

    public TmdbService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _isConfigured = !string.IsNullOrEmpty(configuration["Tmdb:AccessToken"]);
    }

    public async Task<IEnumerable<TmdbMovieSearchDto>> SearchMoviesAsync(string query)
    {
        if (!_isConfigured)
            return Enumerable.Empty<TmdbMovieSearchDto>();

        var response = await _httpClient.GetFromJsonAsync<TmdbSearchResponse<TmdbMovieResult>>(
            $"search/movie?query={Uri.EscapeDataString(query)}&language=cs-CZ",
            JsonOptions);

        return response?.Results.Select(m => new TmdbMovieSearchDto(
                   m.Id,
                   m.Title ?? string.Empty,
                   ParseYear(m.ReleaseDate)))
               ?? Enumerable.Empty<TmdbMovieSearchDto>();
    }

    public async Task<IEnumerable<TmdbTvShowSearchDto>> SearchTvShowsAsync(string query)
    {
        if (!_isConfigured)
            return Enumerable.Empty<TmdbTvShowSearchDto>();

        var response = await _httpClient.GetFromJsonAsync<TmdbSearchResponse<TmdbTvShowResult>>(
            $"search/tv?query={Uri.EscapeDataString(query)}&language=cs-CZ",
            JsonOptions);

        return response?.Results.Select(t => new TmdbTvShowSearchDto(
                   t.Id,
                   t.Name ?? string.Empty,
                   ParseYear(t.FirstAirDate)))
               ?? Enumerable.Empty<TmdbTvShowSearchDto>();
    }

    public async Task<TmdbTvShowSeasonsDto?> GetTvShowSeasonsWithEpisodesAsync(int tmdbId)
    {
        if (!_isConfigured)
            return null;

        var response = await GetOrDefaultAsync<TmdbTvShowDetail>($"tv/{tmdbId}?language=cs-CZ");
        if (response is null)
            return null;

        var released = response.Seasons
            .Where(s => s.SeasonNumber >= 0 && IsReleased(s.AirDate))
            .ToList();

        var seasons = await Task.WhenAll(released.Select(s => GetSeasonWithEpisodesAsync(tmdbId, s.SeasonNumber)));

        return new TmdbTvShowSeasonsDto(response.Id, response.Name, seasons.OfType<TmdbSeasonInfoDto>().ToList());
    }

    public async Task<TmdbMovieSearchDto?> GetNextMovieAsync(int tmdbId)
    {
        if (!_isConfigured)
            return null;

        var movie = await GetOrDefaultAsync<TmdbMovieDetail>($"movie/{tmdbId}");
        var collectionId = movie?.BelongsToCollection?.Id;
        if (collectionId is null)
            return null;

        var collection = await GetOrDefaultAsync<TmdbCollectionDetail>($"collection/{collectionId}?language=cs-CZ");
        if (collection is null)
            return null;

        var dated = collection.Parts
            .Select(p => (Part: p, Date: ParseDate(p.ReleaseDate)))
            .Where(x => x.Date.HasValue && x.Date.Value <= DateOnly.FromDateTime(DateTime.UtcNow))
            .OrderBy(x => x.Date)
            .ToList();

        var index = dated.FindIndex(x => x.Part.Id == tmdbId);
        if (index < 0 || index + 1 >= dated.Count)
            return null;

        var next = dated[index + 1];
        if (string.IsNullOrWhiteSpace(next.Part.Title))
            return null;

        return new TmdbMovieSearchDto(next.Part.Id, next.Part.Title, next.Date?.Year);
    }

    public async Task<List<TmdbSeasonSummaryDto>?> GetReleasedSeasonsAsync(int tmdbId)
    {
        if (!_isConfigured)
            return null;

        var response = await GetOrDefaultAsync<TmdbTvShowDetail>($"tv/{tmdbId}");

        return response?.Seasons
            .Where(s => s.SeasonNumber > 0 && IsReleased(s.AirDate))
            .Select(s => new TmdbSeasonSummaryDto(s.SeasonNumber, s.EpisodeCount))
            .ToList();
    }

    public async Task<TmdbSeasonInfoDto?> GetSeasonWithEpisodesAsync(int tmdbId, int seasonNumber)
    {
        if (!_isConfigured)
            return null;

        var detail = await GetOrDefaultAsync<TmdbSeasonDetail>($"tv/{tmdbId}/season/{seasonNumber}?language=cs-CZ");
        if (detail is null)
            return null;

        var episodes = (detail.Episodes ?? [])
            .Where(e => IsReleased(e.AirDate))
            .Select(e => new TmdbEpisodeInfoDto(e.EpisodeNumber, e.Name))
            .ToList();

        return new TmdbSeasonInfoDto(seasonNumber, ParseYear(detail.AirDate), episodes);
    }

    private async Task<T?> GetOrDefaultAsync<T>(string url)
    {
        using var response = await _httpClient.GetAsync(url);
        if (response.StatusCode == HttpStatusCode.NotFound)
            return default;

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>(JsonOptions);
    }

    // Parses "2008-01-20" -> 2008
    private static int? ParseYear(string? date)
        => int.TryParse(date?.Split('-').FirstOrDefault(), out var year) ? year : null;

    private static DateOnly? ParseDate(string? date)
        => DateOnly.TryParse(date, CultureInfo.InvariantCulture, DateTimeStyles.None, out var d) ? d : null;

    private static bool IsReleased(string? date)
        => DateOnly.TryParse(date, CultureInfo.InvariantCulture, DateTimeStyles.None, out var d)
           && d <= DateOnly.FromDateTime(DateTime.UtcNow);
}
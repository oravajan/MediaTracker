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

    public async Task<TmdbTvShowSeasonsDto?> GetTvShowSeasonsAsync(int tmdbId)
    {
        if (!_isConfigured)
            return null;

        var response = await _httpClient.GetFromJsonAsync<TmdbTvShowDetail>(
            $"tv/{tmdbId}?language=cs-CZ",
            JsonOptions);

        if (response is null)
            return null;

        var releasedSeasons = response.Seasons
            .Where(s => s.SeasonNumber > 0 && IsReleased(s.AirDate))
            .ToList();

        var seasonDetails = await Task.WhenAll(
            releasedSeasons.Select(s => GetSeasonEpisodesAsync(tmdbId, s.SeasonNumber)));

        var seasons = releasedSeasons
            .Select(s => new TmdbSeasonInfoDto(
                s.SeasonNumber,
                ParseYear(s.AirDate),
                seasonDetails.First(d => d.SeasonNumber == s.SeasonNumber).Episodes))
            .ToList();

        return new TmdbTvShowSeasonsDto(response.Id, response.Name, seasons);
    }

    // Fetches episode names for a single season - returns empty list on failure so sync can fall back
    private async Task<(int SeasonNumber, List<TmdbEpisodeInfoDto> Episodes)> GetSeasonEpisodesAsync(
        int tmdbId, int seasonNumber)
    {
        var detail = await _httpClient.GetFromJsonAsync<TmdbSeasonDetail>(
            $"tv/{tmdbId}/season/{seasonNumber}?language=cs-CZ",
            JsonOptions);

        var episodes = detail?.Episodes
            .Where(e => IsReleased(e.AirDate))
            .Select(e => new TmdbEpisodeInfoDto(e.EpisodeNumber, e.Name))
            .ToList() ?? new List<TmdbEpisodeInfoDto>();

        return (seasonNumber, episodes);
    }

    // Parses "2008-01-20" -> 2008
    private static int? ParseYear(string? date)
        => int.TryParse(date?.Split('-').FirstOrDefault(), out var year) ? year : null;
    
    private static bool IsReleased(string? date)
        => DateOnly.TryParse(date, CultureInfo.InvariantCulture, DateTimeStyles.None, out var d)
           && d <= DateOnly.FromDateTime(DateTime.UtcNow);
}
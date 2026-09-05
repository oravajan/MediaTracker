using System.Net.Http.Json;
using System.Text.Json;
using MediaTracker.Application.DTOs;
using MediaTracker.Application.Interfaces;
using MediaTracker.Infrastructure.ExternalServices.Tmdb.Models;
using Microsoft.Extensions.Configuration;

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
                   t.Name ?? string.Empty))
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

        // Filter out specials - season 0 is usually specials/extras
        var seasons = response.Seasons
            .Where(s => s.SeasonNumber > 0)
            .Select(s => new TmdbSeasonInfoDto(
                s.SeasonNumber,
                s.EpisodeCount,
                ParseYear(s.AirDate)))
            .ToList();

        return new TmdbTvShowSeasonsDto(response.Id, response.Name, seasons);
    }

    // Parses "2008-01-20" -> 2008
    private static int? ParseYear(string? date)
        => int.TryParse(date?.Split('-').FirstOrDefault(), out var year) ? year : null;
}
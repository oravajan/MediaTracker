using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MediaTracker.Application.DTOs;
using MediaTracker.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;

namespace MediaTracker.Tests.Integration;

public class MediaControllerTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly TestWebApplicationFactory _factory;

    public MediaControllerTests(TestWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    private void ClearDatabase()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Media.RemoveRange(db.Media);
        db.SaveChanges();
    }

    [Fact]
    public async Task GetAll_WithNoMedia_ReturnsEmptyList()
    {
        ClearDatabase();

        var response = await _client.GetAsync("/api/media");
        var result = await response.Content.ReadFromJsonAsync<List<MediaSummaryDto>>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAll_WithExistingMedia_ReturnsAllMedia()
    {
        ClearDatabase();
        await _client.PostAsJsonAsync("/api/movies",
            new CreateMovieDto("Inception", null, null));
        await _client.PostAsJsonAsync("/api/tvshows",
            new CreateTvShowDto("Breaking Bad", null));

        var response = await _client.GetAsync("/api/media");
        var result = await response.Content.ReadFromJsonAsync<List<MediaSummaryDto>>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task AddMovie_WithValidData_ReturnsCreated()
    {
        var response = await _client.PostAsJsonAsync("/api/movies",
            new CreateMovieDto("Inception", 9, null));

        var result = await response.Content.ReadFromJsonAsync<MovieDto>();

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        result!.Title.Should().Be("Inception");
        result.UserRating.Should().Be(9);
        result.Id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task AddMovie_WithEmptyTitle_ReturnsBadRequest()
    {
        var response = await _client.PostAsJsonAsync("/api/movies",
            new CreateMovieDto("", null, null));

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AddMovie_WithInvalidRating_ReturnsBadRequest()
    {
        var response = await _client.PostAsJsonAsync("/api/movies",
            new CreateMovieDto("Inception", 11, null));

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetMovieById_WithExistingMovie_ReturnsMovie()
    {
        var createResponse = await _client.PostAsJsonAsync("/api/movies",
            new CreateMovieDto("Inception", null, null));
        var created = await createResponse.Content.ReadFromJsonAsync<MovieDto>();

        var response = await _client.GetAsync($"/api/movies/{created!.Id}");
        var result = await response.Content.ReadFromJsonAsync<MovieDto>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result!.Title.Should().Be("Inception");
    }

    [Fact]
    public async Task GetMovieById_WithNonExistingMovie_ReturnsNotFound()
    {
        var response = await _client.GetAsync($"/api/movies/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteMedia_WithExistingMovie_ReturnsNoContent()
    {
        var createResponse = await _client.PostAsJsonAsync("/api/movies",
            new CreateMovieDto("Inception", null, null));
        var created = await createResponse.Content.ReadFromJsonAsync<MovieDto>();

        var response = await _client.DeleteAsync($"/api/media/{created!.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task UpdateMovie_WithValidData_ReturnsUpdatedMovie()
    {
        var createResponse = await _client.PostAsJsonAsync("/api/movies",
            new CreateMovieDto("Inception", null, null));
        var created = await createResponse.Content.ReadFromJsonAsync<MovieDto>();

        var response = await _client.PutAsJsonAsync($"/api/movies/{created!.Id}",
            new UpdateMovieDto("Inception Updated", 8, null, false));
        var result = await response.Content.ReadFromJsonAsync<MovieDto>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result!.Title.Should().Be("Inception Updated");
        result.UserRating.Should().Be(8);
    }

    [Fact]
    public async Task UpdateMovie_WithCircularReference_ReturnsBadRequest()
    {
        var movie1Response = await _client.PostAsJsonAsync("/api/movies",
            new CreateMovieDto("Film 1", null, null));
        var movie1 = await movie1Response.Content.ReadFromJsonAsync<MovieDto>();

        var movie2Response = await _client.PostAsJsonAsync("/api/movies",
            new CreateMovieDto("Film 2", null, movie1!.Id));
        var movie2 = await movie2Response.Content.ReadFromJsonAsync<MovieDto>();

        // movie1 -> movie2 -> movie1 = circular reference
        var response = await _client.PutAsJsonAsync($"/api/movies/{movie1.Id}",
            new UpdateMovieDto("Film 1", null, movie2!.Id, false));

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task WatchMedia_TvShow_MarksEpisodesSequentiallyAcrossSeasons()
    {
        ClearDatabase();

        // Create TvShow
        var tvShowResponse = await _client.PostAsJsonAsync("/api/tvshows",
            new CreateTvShowDto("Breaking Bad", null));
        var tvShow = await tvShowResponse.Content.ReadFromJsonAsync<TvShowDto>();

        // Add Season 1 with 2 episodes
        var season1Response = await _client.PostAsJsonAsync(
            $"/api/tvshows/{tvShow!.Id}/seasons",
            new CreateSeasonDto(1));
        var season1 = await season1Response.Content.ReadFromJsonAsync<SeasonDto>();

        await _client.PostAsJsonAsync(
            $"/api/tvshows/{tvShow.Id}/seasons/{season1!.Id}/episodes",
            new CreateEpisodeDto(1, "Pilot", false));
        await _client.PostAsJsonAsync(
            $"/api/tvshows/{tvShow.Id}/seasons/{season1.Id}/episodes",
            new CreateEpisodeDto(2, "Cat's in the Bag", false));

        // Add Season 2 with 1 episode
        var season2Response = await _client.PostAsJsonAsync(
            $"/api/tvshows/{tvShow.Id}/seasons",
            new CreateSeasonDto(2));
        var season2 = await season2Response.Content.ReadFromJsonAsync<SeasonDto>();

        await _client.PostAsJsonAsync(
            $"/api/tvshows/{tvShow.Id}/seasons/{season2!.Id}/episodes",
            new CreateEpisodeDto(1, "Seven Thirty-Seven", false));

        // Watch 3x - should mark S1E1, S1E2, S2E1 in order
        await _client.PatchAsync($"/api/media/{tvShow.Id}/watch", null);
        await _client.PatchAsync($"/api/media/{tvShow.Id}/watch", null);
        await _client.PatchAsync($"/api/media/{tvShow.Id}/watch", null);

        // Verify final state
        var detailResponse = await _client.GetAsync($"/api/tvshows/{tvShow.Id}");
        var detail = await detailResponse.Content.ReadFromJsonAsync<TvShowDto>();

        detail.Should().NotBeNull();

        var s1 = detail.Seasons.First(s => s.SeasonNumber == 1);
        var s2 = detail.Seasons.First(s => s.SeasonNumber == 2);

        s1.Episodes.All(e => e.IsWatched).Should().BeTrue("all Season 1 episodes should be watched");
        s2.Episodes.First(e => e.EpisodeNumber == 1).IsWatched.Should()
            .BeTrue("Season 2 Episode 1 should be watched after watching all of Season 1");

        // Verify summary counts
        var mediaResponse = await _client.GetAsync("/api/media");
        var media = await mediaResponse.Content.ReadFromJsonAsync<List<MediaSummaryDto>>();
        var summary = media!.First(m => m.Id == tvShow.Id);

        summary.WatchedEpisodeCount.Should().Be(3);
        summary.TotalEpisodeCount.Should().Be(3);
        summary.IsWatched.Should().BeTrue();
    }

    [Fact]
    public async Task MovieChain_ValidChainAllowed_CircularChainRejected()
    {
        ClearDatabase();

        // Create three movies without links
        var film1Response = await _client.PostAsJsonAsync("/api/movies",
            new CreateMovieDto("Spider-Man", null, null));
        var film1 = await film1Response.Content.ReadFromJsonAsync<MovieDto>();

        var film2Response = await _client.PostAsJsonAsync("/api/movies",
            new CreateMovieDto("Spider-Man 2", null, null));
        var film2 = await film2Response.Content.ReadFromJsonAsync<MovieDto>();

        var film3Response = await _client.PostAsJsonAsync("/api/movies",
            new CreateMovieDto("Spider-Man 3", null, null));
        var film3 = await film3Response.Content.ReadFromJsonAsync<MovieDto>();

        // Build valid chain: film1 -> film2 -> film3
        var link1Response = await _client.PutAsJsonAsync($"/api/movies/{film1!.Id}",
            new UpdateMovieDto("Spider-Man", null, film2!.Id, false));
        link1Response.StatusCode.Should().Be(HttpStatusCode.OK, "film1 -> film2 is a valid link");

        var link2Response = await _client.PutAsJsonAsync($"/api/movies/{film2.Id}",
            new UpdateMovieDto("Spider-Man 2", null, film3!.Id, false));
        link2Response.StatusCode.Should().Be(HttpStatusCode.OK, "film2 -> film3 is a valid link");

        // Try to create circular reference: film3 -> film1 would make film1 -> film2 -> film3 -> film1
        var circularResponse = await _client.PutAsJsonAsync($"/api/movies/{film3.Id}",
            new UpdateMovieDto("Spider-Man 3", null, film1.Id, false));
        circularResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest,
            "film3 -> film1 would create a circular reference");

        // Verify film3 still has no NextMovieId after rejected update
        var film3DetailResponse = await _client.GetAsync($"/api/movies/{film3.Id}");
        var film3Detail = await film3DetailResponse.Content.ReadFromJsonAsync<MovieDto>();
        film3Detail!.NextMovieId.Should().BeNull("circular reference was rejected, NextMovieId should remain null");
    }
}
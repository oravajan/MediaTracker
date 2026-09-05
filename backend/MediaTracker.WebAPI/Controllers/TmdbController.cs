using MediaTracker.Application.DTOs;
using MediaTracker.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MediaTracker.WebAPI.Controllers;

[ApiController]
[Route("api/tmdb")]
public class TmdbController : ControllerBase
{
    private readonly ITmdbService _tmdbService;

    public TmdbController(ITmdbService tmdbService)
    {
        _tmdbService = tmdbService;
    }

    [HttpGet("status")]
    public ActionResult<bool> GetStatus()
        => Ok(_tmdbService.IsConfigured);

    [HttpGet("movies/search")]
    public async Task<ActionResult<IEnumerable<TmdbMovieSearchDto>>> SearchMovies(
        [FromQuery] string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest("Query cannot be empty.");

        var results = await _tmdbService.SearchMoviesAsync(query);
        return Ok(results);
    }

    [HttpGet("tvshows/search")]
    public async Task<ActionResult<IEnumerable<TmdbTvShowSearchDto>>> SearchTvShows(
        [FromQuery] string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest("Query cannot be empty.");

        var results = await _tmdbService.SearchTvShowsAsync(query);
        return Ok(results);
    }

    [HttpGet("tvshows/{tmdbId}/seasons")]
    public async Task<ActionResult<TmdbTvShowSeasonsDto>> GetTvShowSeasons(int tmdbId)
    {
        var result = await _tmdbService.GetTvShowSeasonsAsync(tmdbId);
        if (result is null)
            return NotFound();

        return Ok(result);
    }
}
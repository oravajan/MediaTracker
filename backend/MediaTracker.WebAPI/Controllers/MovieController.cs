using MediaTracker.Application.DTOs;
using MediaTracker.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MediaTracker.WebAPI.Controllers;

[ApiController]
[Route("api/movies")]
public class MovieController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MovieController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<MovieDto>> GetById(Guid id)
    {
        var movie = await _movieService.GetByIdAsync(id);
        return Ok(movie);
    }

    [HttpPost]
    public async Task<ActionResult<MovieDto>> Add(CreateMovieDto dto)
    {
        var movie = await _movieService.AddAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = movie.Id }, movie);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<MovieDto>> Update(Guid id, UpdateMovieDto dto)
    {
        var movie = await _movieService.UpdateAsync(id, dto);
        return Ok(movie);
    }
    
    [HttpPatch("{id:guid}/watched")]
    public async Task<ActionResult<MovieDto>> MarkWatched(Guid episodeId, MarkWatchedMovieDto dto)
    {
        var episode = await _movieService.MarkWatchedAsync(episodeId, dto);
        return Ok(episode);
    }
}
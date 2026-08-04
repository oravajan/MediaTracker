using MediaTracker.Application.DTOs;
using MediaTracker.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MediaTracker.WebAPI.Controllers;

[ApiController]
[Route("api/tvshows")]
public class TvShowController : ControllerBase
{
    private readonly ITvShowService _tvShowService;

    public TvShowController(ITvShowService tvShowService)
    {
        _tvShowService = tvShowService;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TvShowDto>> GetById(Guid id)
    {
        var tvShow = await _tvShowService.GetByIdAsync(id);
        return Ok(tvShow);
    }

    [HttpPost]
    public async Task<ActionResult<TvShowDto>> Add(CreateTvShowDto dto)
    {
        var tvShow = await _tvShowService.AddAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = tvShow.Id }, tvShow);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<TvShowDto>> Update(Guid id, UpdateTvShowDto dto)
    {
        var tvShow = await _tvShowService.UpdateAsync(id, dto);
        return Ok(tvShow);
    }
}
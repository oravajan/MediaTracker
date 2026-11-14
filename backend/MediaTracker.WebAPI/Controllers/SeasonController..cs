using MediaTracker.Application.DTOs;
using MediaTracker.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MediaTracker.WebAPI.Controllers;

[ApiController]
[Route("api/tvshows/{tvShowId:guid}/seasons")]
public class SeasonController : ControllerBase
{
    private readonly ITvShowService _tvShowService;

    public SeasonController(ITvShowService tvShowService)
    {
        _tvShowService = tvShowService;
    }

    [HttpPost]
    public async Task<ActionResult<SeasonDto>> Add(Guid tvShowId, CreateSeasonDto dto)
    {
        var season = await _tvShowService.AddSeasonAsync(tvShowId, dto);
        return Created(string.Empty, season);
    }

    [HttpPut("{seasonId:guid}")]
    public async Task<ActionResult<SeasonDto>> Update(Guid tvShowId, Guid seasonId, UpdateSeasonDto dto)
    {
        var season = await _tvShowService.UpdateSeasonAsync(tvShowId, seasonId, dto);
        return Ok(season);
    }

    [HttpDelete("{seasonId:guid}")]
    public async Task<ActionResult> Delete(Guid tvShowId, Guid seasonId)
    {
        await _tvShowService.DeleteSeasonAsync(tvShowId, seasonId);
        return NoContent();
    }
    
    [HttpPatch("{seasonId:guid}/watched")]
    public async Task<ActionResult<SeasonDto>> MarkWatched(Guid tvShowId, Guid seasonId, MarkWatchedSeasonDto dto)
    {
        var season = await _tvShowService.MarkWatchedSeasonAsync(tvShowId, seasonId, dto);
        return Ok(season);
    }
}
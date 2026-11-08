using MediaTracker.Application.DTOs;
using MediaTracker.Application.Interfaces;
using MediaTracker.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace MediaTracker.WebAPI.Controllers;

[ApiController]
[Route("api/media")]
public class MediaController : ControllerBase
{
    private readonly IMediaService _mediaService;
    private readonly ITmdbService _tmdbService;
    private readonly MediaUpdateRunner _updateRunner;

    public MediaController(IMediaService mediaService, ITmdbService tmdbService,
        MediaUpdateRunner updateRunner)
    {
        _mediaService = mediaService;
        _tmdbService = tmdbService;
        _updateRunner = updateRunner;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MediaSummaryDto>>> GetAll()
    {
        var media = await _mediaService.GetAllAsync();
        return Ok(media);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await _mediaService.DeleteAsync(id);
        return NoContent();
    }
    
    [HttpPatch("{id:guid}/watch")]
    public async Task<ActionResult<MediaSummaryDto>> Watch(Guid id)
    {
        var result = await _mediaService.WatchAsync(id);
        return Ok(result);
    }
    
    [HttpPost("update")]
    public ActionResult StartUpdate()
    {
        if (!_tmdbService.IsConfigured)
            return BadRequest(new { error = "TMDB is not configured." });

        return _updateRunner.TryStart()
            ? Accepted()
            : Conflict(new { error = "Update is already running." });
    }

    [HttpGet("update/status")]
    public ActionResult<bool> GetUpdateStatus()
        => Ok(_updateRunner.IsRunning);
}
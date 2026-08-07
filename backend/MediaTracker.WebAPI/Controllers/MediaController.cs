using MediaTracker.Application.DTOs;
using MediaTracker.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MediaTracker.WebAPI.Controllers;

[ApiController]
[Route("api/media")]
public class MediaController : ControllerBase
{
    private readonly IMediaService _mediaService;

    public MediaController(IMediaService mediaService)
    {
        _mediaService = mediaService;
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
}
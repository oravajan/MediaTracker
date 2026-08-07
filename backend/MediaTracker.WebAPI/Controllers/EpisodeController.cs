using MediaTracker.Application.DTOs;
using MediaTracker.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MediaTracker.WebAPI.Controllers;

[ApiController]
[Route("api/tvshows/{tvShowId:guid}/seasons/{seasonId:guid}/episodes")]
public class EpisodeController : ControllerBase
{
    private readonly ITvShowService _tvShowService;

    public EpisodeController(ITvShowService tvShowService)
    {
        _tvShowService = tvShowService;
    }

    [HttpPost]
    public async Task<ActionResult<EpisodeDto>> Add(Guid seasonId, CreateEpisodeDto dto)
    {
        var episode = await _tvShowService.AddEpisodeAsync(seasonId, dto);
        return Created(string.Empty, episode);
    }

    [HttpPut("{episodeId:guid}")]
    public async Task<ActionResult<EpisodeDto>> Update(Guid episodeId, UpdateEpisodeDto dto)
    {
        var episode = await _tvShowService.UpdateEpisodeAsync(episodeId, dto);
        return Ok(episode);
    }

    [HttpDelete("{episodeId:guid}")]
    public async Task<ActionResult> Delete(Guid episodeId)
    {
        await _tvShowService.DeleteEpisodeAsync(episodeId);
        return NoContent();
    }

    [HttpPatch("{episodeId:guid}/watched")]
    public async Task<ActionResult<EpisodeDto>> MarkWatched(Guid episodeId, MarkWatchedEpisodeDto dto)
    {
        var episode = await _tvShowService.MarkWatchedEpisodeAsync(episodeId, dto);
        return Ok(episode);
    }
}
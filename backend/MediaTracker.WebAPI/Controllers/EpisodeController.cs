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
    public async Task<ActionResult<EpisodeDto>> Add(Guid tvShowId, Guid seasonId, CreateEpisodeDto dto)
    {
        var episode = await _tvShowService.AddEpisodeAsync(tvShowId, seasonId, dto);
        return Created(string.Empty, episode);
    }

    [HttpPut("{episodeId:guid}")]
    public async Task<ActionResult<EpisodeDto>> Update(Guid tvShowId, Guid seasonId, Guid episodeId,
        UpdateEpisodeDto dto)
    {
        var episode = await _tvShowService.UpdateEpisodeAsync(tvShowId, seasonId, episodeId, dto);
        return Ok(episode);
    }

    [HttpDelete("{episodeId:guid}")]
    public async Task<ActionResult> Delete(Guid tvShowId, Guid seasonId, Guid episodeId)
    {
        await _tvShowService.DeleteEpisodeAsync(tvShowId, seasonId, episodeId);
        return NoContent();
    }

    [HttpPatch("{episodeId:guid}/watched")]
    public async Task<ActionResult<EpisodeDto>> MarkWatched(Guid tvShowId, Guid seasonId, Guid episodeId,
        MarkWatchedEpisodeDto dto)
    {
        var episode = await _tvShowService.MarkWatchedEpisodeAsync(tvShowId, seasonId, episodeId, dto);
        return Ok(episode);
    }
}
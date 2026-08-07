using MediaTracker.Application.DTOs;
using MediaTracker.Application.Exceptions;
using MediaTracker.Application.Interfaces;
using MediaTracker.Application.Mappers;

namespace MediaTracker.Application.Services;

public class MediaService : IMediaService
{
    private readonly IMediaRepository _mediaRepository;

    public MediaService(IMediaRepository mediaRepository)
    {
        _mediaRepository = mediaRepository;
    }

    public async Task<IEnumerable<MediaSummaryDto>> GetAllAsync()
    {
        var allMedia = await _mediaRepository.GetAllAsync();
        return allMedia.Select(m => m.ToSummaryDto());
    }

    public async Task DeleteAsync(Guid id)
    {
        await _mediaRepository.DeleteAsync(id);
        await _mediaRepository.SaveChangesAsync();
    }

    public async Task<MediaSummaryDto> WatchAsync(Guid id)
    {
        var allMediaWithDetail = await _mediaRepository.GetAllAsync();
        var media = allMediaWithDetail.FirstOrDefault(m => m.Id == id);
        if (media is null)
            throw new NotFoundException($"Media with id {id} was not found.");

        media.Watch();
        await _mediaRepository.SaveChangesAsync();

        return media.ToSummaryDto();
    }
}
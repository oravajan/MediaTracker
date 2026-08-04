using MediaTracker.Application.DTOs;
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
}
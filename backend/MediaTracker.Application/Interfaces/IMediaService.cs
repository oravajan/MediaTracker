using MediaTracker.Application.DTOs;

namespace MediaTracker.Application.Interfaces;

public interface IMediaService
{
    Task<IEnumerable<MediaSummaryDto>> GetAllAsync();
    Task DeleteAsync(Guid id);
    Task<MediaSummaryDto> WatchAsync(Guid id);
}
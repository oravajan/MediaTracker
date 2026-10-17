using MediaTracker.Application.DTOs;
using MediaTracker.Application.Exceptions;
using MediaTracker.Application.Interfaces;
using MediaTracker.Application.Mappers;
using MediaTracker.Domain.Entities;

namespace MediaTracker.Application.Services;

public class MediaService : IMediaService
{
    private readonly IMediaRepository _mediaRepository;
    private readonly INextMovieLinker _nextMovieLinker;

    public MediaService(IMediaRepository mediaRepository, INextMovieLinker nextMovieLinker)
    {
        _mediaRepository = mediaRepository;
        _nextMovieLinker = nextMovieLinker;
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
        var media = await _mediaRepository.GetByIdAsync(id);
        if (media is null)
            throw new NotFoundException($"Media with id {id} was not found.");

        var wasUnwatchedMovie = media is Movie { IsWatched: false };
        
        media.Watch();
        await _mediaRepository.SaveChangesAsync();
        
        if (wasUnwatchedMovie && media is Movie { IsWatched: true } movie)
            await _nextMovieLinker.TryLinkAsync(movie);

        return media.ToSummaryDto();
    }
}
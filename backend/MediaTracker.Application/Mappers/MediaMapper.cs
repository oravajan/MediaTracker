using MediaTracker.Application.DTOs;
using MediaTracker.Domain.Entities;

namespace MediaTracker.Application.Mappers;

public static class MediaMapper
{
    public static MediaSummaryDto ToSummaryDto(this Media media) => media switch
    {
        Movie m => new MediaSummaryDto(m.Id, m.Title, "Movie", m.UserRating, m.IsWatched, null, null,
            m.WatchedAt, m.CreatedAt),
        TvShow t => new MediaSummaryDto(t.Id, t.Title, "TvShow", t.UserRating, t.IsWatched, t.WatchedEpisodeCount,
            t.TotalEpisodeCount, t.LastWatchedAt, t.CreatedAt),
        _ => throw new InvalidOperationException($"Unknown media type: {media.GetType().Name}")
    };
}
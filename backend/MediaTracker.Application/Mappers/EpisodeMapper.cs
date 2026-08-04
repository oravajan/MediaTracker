using MediaTracker.Application.DTOs;
using MediaTracker.Domain.Entities;

namespace MediaTracker.Application.Mappers;

public static class EpisodeMapper
{
    public static Episode ToEntity(this CreateEpisodeDto dto)
    {
        return new Episode(Guid.Empty, dto.EpisodeNumber, dto.Title, dto.IsWatched);
    }

    public static EpisodeDto ToDto(this Episode episode)
    {
        return new EpisodeDto(episode.Id, episode.EpisodeNumber, episode.Title, episode.IsWatched);
    }
}
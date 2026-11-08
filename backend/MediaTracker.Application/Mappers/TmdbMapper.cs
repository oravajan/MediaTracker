using MediaTracker.Application.DTOs;
using MediaTracker.Domain.Entities;

namespace MediaTracker.Application.Mappers;

public static class TmdbMapper
{
    public static Season ToEntity(this TmdbSeasonInfoDto dto)
    {
        var season = new Season(dto.SeasonNumber, dto.ReleaseYear);

        foreach (var episode in dto.Episodes)
            season.AddEpisode(new Episode(episode.EpisodeNumber, episode.Title, false));

        return season;
    }
}
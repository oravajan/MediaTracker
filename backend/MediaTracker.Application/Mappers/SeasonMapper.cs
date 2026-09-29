using MediaTracker.Application.DTOs;
using MediaTracker.Domain.Entities;

namespace MediaTracker.Application.Mappers;

public static class SeasonMapper
{
    public static Season ToEntity(this CreateSeasonDto dto)
    {
        return new Season(dto.SeasonNumber, dto.ReleaseYear);
    }


    public static SeasonDto ToDto(this Season season)
    {
        return new SeasonDto(season.Id, season.SeasonNumber,
            season.Episodes
                .OrderBy(e => e.EpisodeNumber)
                .Select(e => e.ToDto())
                .ToList(),
            season.ReleaseYear);
    }
}
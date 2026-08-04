using MediaTracker.Application.DTOs;
using MediaTracker.Domain.Entities;

namespace MediaTracker.Application.Mappers;

public static class SeasonMapper
{
    public static Season ToEntity(this CreateSeasonDto dto)
    {
        return new Season(Guid.Empty, dto.SeasonNumber, new List<Episode>());
    }


    public static SeasonDto ToDto(this Season season)
    {
        return new SeasonDto(season.Id, season.SeasonNumber, season.Episodes.Select(e => e.ToDto()).ToList());
    }
}
using MediaTracker.Application.DTOs;
using MediaTracker.Domain.Entities;

namespace MediaTracker.Application.Mappers;

public static class TvShowMapper
{
    public static TvShow ToEntity(this CreateTvShowDto dto)
    {
        return new TvShow(dto.Title, dto.UserRating, dto.TmdbId);
    }

    public static TvShowDto ToDto(this TvShow tvShow)
    {
        return new TvShowDto(tvShow.Id, tvShow.Title, tvShow.UserRating,
            tvShow.Seasons
                .OrderBy(s => s.SeasonNumber)
                .Select(s => s.ToDto())
                .ToList(),
            tvShow.TmdbId);
    }
}
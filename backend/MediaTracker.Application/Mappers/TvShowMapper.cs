using MediaTracker.Application.DTOs;
using MediaTracker.Domain.Entities;

namespace MediaTracker.Application.Mappers;

public static class TvShowMapper
{
    public static TvShow ToEntity(this CreateTvShowDto dto)
    {
        return new TvShow(Guid.Empty, dto.Title, dto.UserRating, new List<Season>());
    }

    public static TvShowDto ToDto(this TvShow tvShow)
    {
        return new TvShowDto(tvShow.Id, tvShow.Title,
            tvShow.UserRating, tvShow.Seasons.Select(s => s.ToDto()).ToList());
    }
}
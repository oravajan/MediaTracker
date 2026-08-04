using MediaTracker.Domain.Exceptions;

namespace MediaTracker.Domain.Entities;

public class TvShow : Media
{
    public List<Season> Seasons { get; private set; } = new();

    public TvShow(Guid id, string title, int? userRating, List<Season> seasons) : base(id, title,
        userRating)
    {
        Seasons = seasons;
    }

    private TvShow()
    {
    }

    public void AddSeason(Season season)
    {
        if (season is null)
            throw new DomainException("Season cannot be null.");

        if (Seasons.Any(s => s.SeasonNumber == season.SeasonNumber))
            throw new DomainException($"Season {season.SeasonNumber} already exists.");

        Seasons.Add(season);
    }
}
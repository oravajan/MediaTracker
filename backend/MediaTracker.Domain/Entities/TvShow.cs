using MediaTracker.Domain.Exceptions;

namespace MediaTracker.Domain.Entities;

public class TvShow : Media
{
    public List<Season> Seasons { get; private set; } = new();
    public int TotalEpisodeCount => Seasons.SelectMany(s => s.Episodes).Count();
    public int WatchedEpisodeCount => Seasons.SelectMany(s => s.Episodes).Count(e => e.IsWatched);
    public bool IsWatched => TotalEpisodeCount > 0 && WatchedEpisodeCount == TotalEpisodeCount;

    public TvShow(Guid id, string title, int? userRating, List<Season> seasons) : base(id, title,
        userRating)
    {
        Seasons = seasons;
    }

    private TvShow()
    {
    }

    public override void Watch()
    {
        var allEpisodes = Seasons
            .OrderBy(s => s.SeasonNumber)
            .SelectMany(s => s.Episodes
                .OrderBy(e => e.EpisodeNumber)
                .Select(e => new { Season = s, Episode = e }))
            .ToList();

        if (allEpisodes.Count == 0)
            return;
        
        var lastWatched = allEpisodes
            .LastOrDefault(x => x.Episode.IsWatched);
        
        if (lastWatched is null)
        {
            allEpisodes.First().Episode.MarkWatched(true);
            return;
        }
        
        var lastWatchedIndex = allEpisodes.IndexOf(lastWatched);
        var nextEpisode = allEpisodes.ElementAtOrDefault(lastWatchedIndex + 1);

        nextEpisode?.Episode.MarkWatched(true);
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
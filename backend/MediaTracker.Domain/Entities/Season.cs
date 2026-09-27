using MediaTracker.Domain.Exceptions;

namespace MediaTracker.Domain.Entities;

public class Season
{
    private readonly List<Episode> _episodes = new();
    
    public Guid Id { get; private set; }
    public int SeasonNumber { get; private set; }
    public IReadOnlyList<Episode> Episodes => _episodes.AsReadOnly();

    public Season(int seasonNumber)
    {
        ValidateSeasonNumber(seasonNumber);
        Id = Guid.Empty;
        SeasonNumber = seasonNumber;
    }

    public Season() { }

    public void Update(int seasonNumber)
    {
        ValidateSeasonNumber(seasonNumber);
        SeasonNumber = seasonNumber;
    }

    public void AddEpisode(Episode episode)
    {
        if (episode is null)
            throw new DomainException("Episode cannot be null.");

        if (Episodes.Any(e => e.EpisodeNumber == episode.EpisodeNumber))
            throw new DomainException($"Episode {episode.EpisodeNumber} already exists in this season.");

        _episodes.Add(episode);
    }

    private static void ValidateSeasonNumber(int seasonNumber)
    {
        if (seasonNumber < 0)
            throw new DomainException("Season number must be greater than or equal to 0.");
    }
}
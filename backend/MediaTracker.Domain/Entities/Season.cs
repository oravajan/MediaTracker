using MediaTracker.Domain.Exceptions;

namespace MediaTracker.Domain.Entities;

public class Season
{
    public Guid Id { get; private set; }
    public int SeasonNumber { get; private set; }
    public List<Episode> Episodes { get; private set; } = new();

    public Season(Guid id, int seasonNumber, List<Episode> episodes)
    {
        ValidateSeasonNumber(seasonNumber);

        Id = id;
        SeasonNumber = seasonNumber;
        Episodes = episodes;
    }

    public Season()
    {
    }

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

        Episodes.Add(episode);
    }

    private static void ValidateSeasonNumber(int seasonNumber)
    {
        if (seasonNumber < 1)
            throw new DomainException("Season number must be greater than or equal to 1.");
    }
}
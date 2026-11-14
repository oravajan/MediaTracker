using MediaTracker.Domain.Exceptions;

namespace MediaTracker.Domain.Entities;

public class Episode
{
    public Guid Id { get; private set; }
    public int EpisodeNumber { get; private set; }
    public string? Title { get; private set; }
    public bool IsWatched { get; private set; }
    public DateTime? WatchedAt { get; private set; }

    public Episode(int episodeNumber, string? title, bool isWatched)
    {
        ValidateEpisodeNumber(episodeNumber);
        Id = Guid.Empty;
        EpisodeNumber = episodeNumber;
        Title = title;
        MarkWatched(isWatched);
    }

    public Episode() { }

    public void Update(int episodeNumber, string? title, bool isWatched)
    {
        ValidateEpisodeNumber(episodeNumber);
        EpisodeNumber = episodeNumber;
        Title = title;
        MarkWatched(isWatched);
    }

    public void MarkWatched(bool isWatched)
    {
        if (isWatched == IsWatched)
            return;

        IsWatched = isWatched;
        WatchedAt = isWatched ? DateTime.UtcNow : null;
    }

    private static void ValidateEpisodeNumber(int episodeNumber)
    {
        if (episodeNumber < 1)
            throw new DomainException("Episode number must be greater than or equal to 1.");
    }
}
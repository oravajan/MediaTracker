using MediaTracker.Domain.Exceptions;

namespace MediaTracker.Domain.Entities;

public abstract class Media
{
    public const int TitleMaxLength = 255;

    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public int? UserRating { get; private set; }
    public int? TmdbId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    protected Media(string title, int? userRating, int? tmdbId)
    {
        ValidateTitle(title);
        ValidateUserRating(userRating);

        Id = Guid.Empty;
        Title = title;
        UserRating = userRating;
        TmdbId = tmdbId;
        CreatedAt = DateTime.UtcNow;
    }

    protected Media() { }

    public abstract void Watch();

    public void Update(string title, int? userRating, int? tmdbId)
    {
        ValidateTitle(title);
        ValidateUserRating(userRating);

        Title = title;
        UserRating = userRating;
        TmdbId = tmdbId;
    }

    private static void ValidateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Title cannot be empty.");

        if (title.Length > TitleMaxLength)
            throw new DomainException($"Title cannot be longer than {TitleMaxLength} characters.");
    }

    private static void ValidateUserRating(int? userRating)
    {
        if (userRating is < 1 or > 10)
            throw new DomainException("Rating must be between 1 and 10.");
    }
}
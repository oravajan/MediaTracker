using MediaTracker.Domain.Exceptions;

namespace MediaTracker.Domain.Entities;

public abstract class Media
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public int? UserRating { get; private set; }

    public Media(Guid id, string title, int? userRating)
    {
        ValidateTitle(title);
        ValidateUserRating(userRating);

        Id = id;
        Title = title;
        UserRating = userRating;
    }

    protected Media()
    {
    }

    public void Update(string title, int? userRating)
    {
        ValidateTitle(title);
        ValidateUserRating(userRating);

        Title = title;
        UserRating = userRating;
    }

    private static void ValidateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Title cannot be empty.");
    }

    private static void ValidateUserRating(int? userRating)
    {
        if (userRating is < 1 or > 10)
            throw new DomainException("Rating must be between 1 and 10.");
    }
}
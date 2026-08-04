using MediaTracker.Domain.Exceptions;

namespace MediaTracker.Domain.Entities;

public class Movie : Media
{
    public Guid? NextMovieId { get; private set; }
    public Movie? NextMovie { get; private set; }

    public Movie(Guid id, string title, int? userRating, Guid? nextMovieId, Movie? nextMovie) : base(id, title,
        userRating)
    {
        ValidateNextMovieId(nextMovieId);

        NextMovieId = nextMovieId;
        NextMovie = nextMovie;
    }

    private Movie()
    {
    }

    public void Update(string title, int? userRating, Guid? nextMovieId)
    {
        base.Update(title, userRating);

        if (nextMovieId == Id)
            throw new DomainException("Movie cannot reference itself as next movie.");

        ValidateNextMovieId(nextMovieId);
        NextMovieId = nextMovieId;
    }

    private void ValidateNextMovieId(Guid? nextMovieId)
    {
        if (nextMovieId == Id)
            throw new DomainException("Movie cannot reference itself as next movie.");
    }
}
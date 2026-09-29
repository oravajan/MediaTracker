using MediaTracker.Domain.Exceptions;

namespace MediaTracker.Domain.Entities;

public class Movie : Media
{
    public Guid? NextMovieId { get; private set; }
    public Movie? NextMovie { get; private set; }
    public bool IsWatched { get; private set; }
    public int? ReleaseYear { get; private set; }

    public Movie(string title, int? userRating, Guid? nextMovieId, bool isWatched,
        int? tmdbId, int? releaseYear) : base(title, userRating, tmdbId)
    {
        ValidateNextMovieId(nextMovieId);
        
        NextMovieId = nextMovieId;
        NextMovie = null;
        IsWatched = isWatched;
        ReleaseYear = releaseYear;
    }

    private Movie() { }

    public override void Watch()
    {
        IsWatched = true;
    }

    public void Update(string title, int? userRating, Guid? nextMovieId, bool isWatched, int? tmdbId, int? releaseYear)
    {
        base.Update(title, userRating, tmdbId);

        ValidateNextMovieId(nextMovieId);
        NextMovieId = nextMovieId;
        IsWatched = isWatched;
        ReleaseYear = releaseYear;
    }

    public void MarkWatched(bool isWatched)
    {
        IsWatched = isWatched;
    }

    private void ValidateNextMovieId(Guid? nextMovieId)
    {
        if (nextMovieId == Guid.Empty)
            throw new DomainException("Next movie id cannot be Guid.Empty.");
        
        if (nextMovieId == Id)
            throw new DomainException("Movie cannot reference itself as next movie.");
    }
}
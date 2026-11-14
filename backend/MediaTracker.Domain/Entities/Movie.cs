using MediaTracker.Domain.Exceptions;

namespace MediaTracker.Domain.Entities;

public class Movie : Media
{
    public Guid? NextMovieId { get; private set; }
    public Movie? NextMovie { get; private set; }
    public bool IsWatched { get; private set; }
    public DateTime? WatchedAt { get; private set; }
    public int? ReleaseYear { get; private set; }

    public Movie(string title, int? userRating, Guid? nextMovieId, bool isWatched,
        int? tmdbId, int? releaseYear) : base(title, userRating, tmdbId)
    {
        ValidateNextMovieId(nextMovieId);

        NextMovieId = nextMovieId;
        NextMovie = null;
        MarkWatched(isWatched);
        ReleaseYear = releaseYear;
    }

    private Movie() { }

    public override void Watch()
    {
        MarkWatched(true);
    }

    public void Update(string title, int? userRating, Guid? nextMovieId, bool isWatched, int? tmdbId, int? releaseYear)
    {
        base.Update(title, userRating, tmdbId);

        ValidateNextMovieId(nextMovieId);
        NextMovieId = nextMovieId;
        MarkWatched(isWatched);
        ReleaseYear = releaseYear;
    }

    public void MarkWatched(bool isWatched)
    {
        if (isWatched == IsWatched)
            return;

        IsWatched = isWatched;
        WatchedAt = isWatched ? DateTime.UtcNow : null;
    }
    
    public void SetNextMovie(Guid nextMovieId)
    {
        ValidateNextMovieId(nextMovieId);
        NextMovieId = nextMovieId;
    }

    private void ValidateNextMovieId(Guid? nextMovieId)
    {
        if (nextMovieId == Guid.Empty)
            throw new DomainException("Next movie id cannot be Guid.Empty.");
        
        if (nextMovieId == Id)
            throw new DomainException("Movie cannot reference itself as next movie.");
    }
}
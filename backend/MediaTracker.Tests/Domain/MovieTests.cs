using FluentAssertions;
using MediaTracker.Domain.Entities;
using MediaTracker.Domain.Exceptions;

namespace MediaTracker.Tests.Domain;

public class MovieTests
{
    private static Movie CreateMovie(
        string title = "Inception",
        int? userRating = null,
        Guid? nextMovieId = null,
        bool isWatched = false)
        => new(title, userRating, nextMovieId, isWatched, null);

    [Fact]
    public void Update_WithValidData_UpdatesProperties()
    {
        var movie = CreateMovie();

        movie.Update("Inception 2", 9, null, true, null);

        movie.Title.Should().Be("Inception 2");
        movie.UserRating.Should().Be(9);
        movie.IsWatched.Should().Be(true);
    }

    [Fact]
    public void Update_WithEmptyTitle_ThrowsDomainException()
    {
        var movie = CreateMovie();

        var act = () => movie.Update("", null, null, false, null);

        act.Should().Throw<DomainException>()
            .WithMessage("*empty*");
    }

    [Fact]
    public void Update_WithRatingBelowRange_ThrowsDomainException()
    {
        var movie = CreateMovie();

        var act = () => movie.Update("Inception", 0, null, false, null);

        act.Should().Throw<DomainException>()
            .WithMessage("*between 1 and 10*");
    }

    [Fact]
    public void Update_WithRatingAboveRange_ThrowsDomainException()
    {
        var movie = CreateMovie();

        var act = () => movie.Update("Inception", 11, null, false, null);

        act.Should().Throw<DomainException>()
            .WithMessage("*between 1 and 10*");
    }

    [Fact]
    public void Update_WithSelfReference_ThrowsDomainException()
    {
        var movie = CreateMovie();

        var act = () => movie.Update("Inception", null, movie.Id, false, null);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Watch_SetsIsWatchedToTrue()
    {
        var movie = CreateMovie();

        movie.Watch();

        movie.IsWatched.Should().BeTrue();
    }

    [Fact]
    public void Constructor_WithEmptyTitle_ThrowsDomainException()
    {
        var act = () => new Movie("", null, null, false, null);

        act.Should().Throw<DomainException>()
            .WithMessage("*empty*");
    }

    [Fact]
    public void Constructor_WithEmptyNextMovieId_ThrowsDomainException()
    {
        var id = Guid.Empty;

        var act = () => new Movie("Inception", null, id, false, null);

        act.Should().Throw<DomainException>();
    }
}
using FluentAssertions;
using MediaTracker.Domain.Entities;
using MediaTracker.Domain.Exceptions;

namespace MediaTracker.Tests.Domain;

public class TvShowTests
{
    private static Episode CreateEpisode(int number, bool isWatched = false)
        => new(number, $"Episode {number}", isWatched);

    private static Season CreateSeason(int number)
        => new(number);

    private static TvShow CreateTvShow()
        => new("Breaking Bad", null, null);

    [Fact]
    public void AddSeason_WithValidSeason_AddsSeason()
    {
        var tvShow = CreateTvShow();
        var season = CreateSeason(1);

        tvShow.AddSeason(season);

        tvShow.Seasons.Should().HaveCount(1);
    }

    [Fact]
    public void AddSeason_WithDuplicateSeasonNumber_ThrowsDomainException()
    {
        var tvShow = CreateTvShow();
        tvShow.AddSeason(CreateSeason(1));

        var act = () => tvShow.AddSeason(CreateSeason(1));

        act.Should().Throw<DomainException>()
            .WithMessage("*already exists*");
    }

    [Fact]
    public void IsWatched_WithNoEpisodes_ReturnsFalse()
    {
        var tvShow = CreateTvShow();

        tvShow.IsWatched.Should().BeFalse();
    }

    [Fact]
    public void IsWatched_WithAllEpisodesWatched_ReturnsTrue()
    {
        var season = CreateSeason(1);
        season.AddEpisode(CreateEpisode(1, isWatched: true));
        season.AddEpisode(CreateEpisode(2, isWatched: true));
        var tvShow = CreateTvShow();
        tvShow.AddSeason(season);

        tvShow.IsWatched.Should().BeTrue();
    }

    [Fact]
    public void IsWatched_WithSomeEpisodesWatched_ReturnsFalse()
    {
        var season = CreateSeason(1);
        season.AddEpisode(CreateEpisode(1, isWatched: true));
        season.AddEpisode(CreateEpisode(2, isWatched: false));
        var tvShow = CreateTvShow();
        tvShow.AddSeason(season);

        tvShow.IsWatched.Should().BeFalse();
    }

    [Fact]
    public void Watch_WithNoEpisodes_DoesNothing()
    {
        var tvShow = CreateTvShow();

        var act = () => tvShow.Watch();

        act.Should().NotThrow();
        tvShow.WatchedEpisodeCount.Should().Be(0);
    }

    [Fact]
    public void Watch_WithNoWatchedEpisodes_MarksFirstEpisode()
    {
        var season = CreateSeason(1);
        season.AddEpisode(CreateEpisode(1));
        season.AddEpisode(CreateEpisode(2));
        var tvShow = CreateTvShow();
        tvShow.AddSeason(season);

        tvShow.Watch();

        tvShow.WatchedEpisodeCount.Should().Be(1);
        season.Episodes[0].IsWatched.Should().BeTrue();
        season.Episodes[1].IsWatched.Should().BeFalse();
    }

    [Fact]
    public void Watch_MarksNextEpisodeAfterLastWatched()
    {
        var season = CreateSeason(1);
        season.AddEpisode(CreateEpisode(1, isWatched: true));
        season.AddEpisode(CreateEpisode(2, isWatched: false));
        season.AddEpisode(CreateEpisode(3, isWatched: false));
        var tvShow = CreateTvShow();
        tvShow.AddSeason(season);

        tvShow.Watch();

        season.Episodes[0].IsWatched.Should().BeTrue();
        season.Episodes[1].IsWatched.Should().BeTrue();
        season.Episodes[2].IsWatched.Should().BeFalse();
    }

    [Fact]
    public void Watch_AtEndOfSeason_MarksFirstEpisodeOfNextSeason()
    {
        var season1 = CreateSeason(1);
        season1.AddEpisode(CreateEpisode(1, isWatched: true));
        
        var season2 = CreateSeason(2);
        season2.AddEpisode(CreateEpisode(1, isWatched: false));

        var tvShow = CreateTvShow();
        tvShow.AddSeason(season1);
        tvShow.AddSeason(season2);

        tvShow.Watch();

        season2.Episodes[0].IsWatched.Should().BeTrue();
    }

    [Fact]
    public void Watch_WhenAllEpisodesWatched_DoesNothing()
    {
        var season = CreateSeason(1);
        season.AddEpisode(CreateEpisode(1, isWatched: true));
        season.AddEpisode(CreateEpisode(2, isWatched: true));
        
        var tvShow = CreateTvShow();
        tvShow.AddSeason(season);

        tvShow.WatchedEpisodeCount.Should().Be(2);
        tvShow.Watch();
        tvShow.WatchedEpisodeCount.Should().Be(2);
    }

    [Fact]
    public void WatchedEpisodeCount_ReturnsCorrectCount()
    {
        var season = CreateSeason(1);
        season.AddEpisode(CreateEpisode(1, isWatched: true));
        season.AddEpisode(CreateEpisode(2, isWatched: false));
        season.AddEpisode(CreateEpisode(3, isWatched: true));
        var tvShow = CreateTvShow();
        tvShow.AddSeason(season);

        tvShow.WatchedEpisodeCount.Should().Be(2);
        tvShow.TotalEpisodeCount.Should().Be(3);
    }

    [Fact]
    public void Watch_WhenLastEpisodeWatched_DoesNothing()
    {
        var season1 = CreateSeason(1);
        season1.AddEpisode(CreateEpisode(1, isWatched: false));
        season1.AddEpisode(CreateEpisode(2, isWatched: false));
        season1.AddEpisode(CreateEpisode(3, isWatched: false));
        
        var season2 = CreateSeason(2);
        season2.AddEpisode(CreateEpisode(1, isWatched: true));

        var tvShow = CreateTvShow();
        tvShow.AddSeason(season1);
        tvShow.AddSeason(season2);

        tvShow.Watch();

        season1.Episodes[0].IsWatched.Should().BeFalse();
        season1.Episodes[1].IsWatched.Should().BeFalse();
        season1.Episodes[2].IsWatched.Should().BeFalse();
        season2.Episodes[0].IsWatched.Should().BeTrue();
    }
}
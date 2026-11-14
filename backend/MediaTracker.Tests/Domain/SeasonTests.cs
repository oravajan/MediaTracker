using FluentAssertions;
using MediaTracker.Domain.Entities;
using MediaTracker.Domain.Exceptions;

namespace MediaTracker.Tests.Domain;

public class SeasonTests
{
    private static Episode CreateEpisode(int number, bool isWatched = false)
        => new(number, $"Episode {number}", isWatched);

    private static Season CreateSeason(int number = 1)
        => new(number, 2000);

    [Fact]
    public void Constructor_WithValidData_CreatesSeason()
    {
        var season = CreateSeason(1);

        season.SeasonNumber.Should().Be(1);
        season.Episodes.Should().BeEmpty();
    }

    [Fact]
    public void Constructor_WithInvalidSeasonNumber_ThrowsDomainException()
    {
        var act = () => new Season(-1, 2000);

        act.Should().Throw<DomainException>()
            .WithMessage("*greater than or equal to 0*");
    }

    [Fact]
    public void UpdateEpisode_WithValidData_UpdatesProperties()
    {
        var season = CreateSeason();
        var episode = CreateEpisode(1);
        season.AddEpisode(episode);

        season.UpdateEpisode(episode, 2, "Updated Title", true);

        episode.EpisodeNumber.Should().Be(2);
        episode.Title.Should().Be("Updated Title");
        episode.IsWatched.Should().BeTrue();
    }

    [Fact]
    public void UpdateEpisode_WithInvalidEpisodeNumber_ThrowsDomainException()
    {
        var season = CreateSeason();
        var episode = CreateEpisode(1);
        season.AddEpisode(episode);

        var act = () => season.UpdateEpisode(episode, 0, "Title", false);

        act.Should().Throw<DomainException>()
            .WithMessage("*greater than or equal to 1*");
    }

    [Fact]
    public void AddEpisode_WithValidEpisode_AddsEpisode()
    {
        var season = CreateSeason();
        var episode = CreateEpisode(1);

        season.AddEpisode(episode);

        season.Episodes.Should().HaveCount(1);
    }

    [Fact]
    public void AddEpisode_WithDuplicateEpisodeNumber_ThrowsDomainException()
    {
        var season = CreateSeason();
        season.AddEpisode(CreateEpisode(1));

        var act = () => season.AddEpisode(CreateEpisode(1));

        act.Should().Throw<DomainException>()
            .WithMessage("*already exists*");
    }
}
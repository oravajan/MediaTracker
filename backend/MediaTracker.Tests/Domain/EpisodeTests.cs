using FluentAssertions;
using MediaTracker.Domain.Entities;
using MediaTracker.Domain.Exceptions;

namespace MediaTracker.Tests.Domain;

public class EpisodeTests
{
    private static Episode CreateEpisode(
        int number = 1,
        string? title = "Pilot",
        bool isWatched = false)
        => new(number, title, isWatched);

    [Fact]
    public void Constructor_WithValidData_CreatesEpisode()
    {
        var episode = CreateEpisode(1, "Pilot");

        episode.EpisodeNumber.Should().Be(1);
        episode.Title.Should().Be("Pilot");
        episode.IsWatched.Should().BeFalse();
    }

    [Fact]
    public void Constructor_WithInvalidEpisodeNumber_ThrowsDomainException()
    {
        var act = () => new Episode(0, "Pilot", false);

        act.Should().Throw<DomainException>()
            .WithMessage("*greater than or equal to 1*");
    }

    [Fact]
    public void Constructor_WithNullTitle_CreatesEpisode()
    {
        var episode = CreateEpisode(1, null);

        episode.Title.Should().BeNull();
    }

    [Fact]
    public void Update_WithValidData_UpdatesProperties()
    {
        var episode = CreateEpisode(1, "Pilot");

        episode.Update(2, "Updated Title", true);

        episode.EpisodeNumber.Should().Be(2);
        episode.Title.Should().Be("Updated Title");
        episode.IsWatched.Should().BeTrue();
    }

    [Fact]
    public void Update_WithInvalidEpisodeNumber_ThrowsDomainException()
    {
        var episode = CreateEpisode();

        var act = () => episode.Update(0, "Title", false);

        act.Should().Throw<DomainException>()
            .WithMessage("*greater than or equal to 1*");
    }

    [Fact]
    public void MarkWatched_SetsIsWatchedCorrectly()
    {
        var episode = CreateEpisode(isWatched: false);

        episode.MarkWatched(true);

        episode.IsWatched.Should().BeTrue();
    }

    [Fact]
    public void MarkWatched_CanUnmarkWatched()
    {
        var episode = CreateEpisode(isWatched: true);

        episode.MarkWatched(false);

        episode.IsWatched.Should().BeFalse();
    }
}
using FluentAssertions;
using MediaTracker.Domain.Entities;
using MediaTracker.Domain.Exceptions;

namespace MediaTracker.Tests.Domain;

public class SeasonTests
{
    private static Episode CreateEpisode(int number, bool isWatched = false)
        => new(Guid.NewGuid(), number, $"Episode {number}", isWatched);

    private static Season CreateSeason(int number = 1)
        => new(Guid.NewGuid(), number, new List<Episode>());

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
        var act = () => new Season(Guid.NewGuid(), 0, new List<Episode>());

        act.Should().Throw<DomainException>()
            .WithMessage("*greater than or equal to 1*");
    }

    [Fact]
    public void Update_WithValidSeasonNumber_UpdatesSeasonNumber()
    {
        var season = CreateSeason(1);

        season.Update(2);

        season.SeasonNumber.Should().Be(2);
    }

    [Fact]
    public void Update_WithInvalidSeasonNumber_ThrowsDomainException()
    {
        var season = CreateSeason(1);

        var act = () => season.Update(0);

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
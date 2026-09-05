using FluentAssertions;
using MediaTracker.Application.DTOs;
using MediaTracker.Application.Exceptions;
using MediaTracker.Application.Interfaces;
using MediaTracker.Application.Services;
using MediaTracker.Domain.Entities;
using MediaTracker.Domain.Exceptions;
using Moq;

namespace MediaTracker.Tests.Application;

public class MovieServiceTests
{
    private readonly Mock<IMediaRepository> _repositoryMock;
    private readonly MovieService _service;

    public MovieServiceTests()
    {
        _repositoryMock = new Mock<IMediaRepository>();
        _service = new MovieService(_repositoryMock.Object);
    }

    private static Movie CreateMovie(
        string title = "Inception",
        int? userRating = null,
        Guid? nextMovieId = null,
        bool isWatched = false)
        => new(Guid.NewGuid(), title, userRating, nextMovieId, null, isWatched, null);

    [Fact]
    public async Task GetByIdAsync_WithExistingMovie_ReturnsMovieDto()
    {
        var movie = CreateMovie("Inception", 9);
        _repositoryMock
            .Setup(r => r.GetMovieByIdAsync(movie.Id))
            .ReturnsAsync(movie);

        var result = await _service.GetByIdAsync(movie.Id);

        result.Should().NotBeNull();
        result.Title.Should().Be("Inception");
        result.UserRating.Should().Be(9);
    }

    [Fact]
    public async Task GetByIdAsync_WithNonExistingMovie_ThrowsNotFoundException()
    {
        var id = Guid.NewGuid();
        _repositoryMock
            .Setup(r => r.GetMovieByIdAsync(id))
            .ReturnsAsync((Movie?)null);

        var act = async () => await _service.GetByIdAsync(id);

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"*{id}*");
    }

    [Fact]
    public async Task AddAsync_WithValidDto_AddsMovieAndReturnsDto()
    {
        var dto = new CreateMovieDto("Inception", null, null, null);
        _repositoryMock
            .Setup(r => r.AddMovieAsync(It.IsAny<Movie>()))
            .Returns(Task.CompletedTask);
        _repositoryMock
            .Setup(r => r.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        var result = await _service.AddAsync(dto);

        result.Should().NotBeNull();
        result.Title.Should().Be("Inception");
        _repositoryMock.Verify(r => r.AddMovieAsync(It.IsAny<Movie>()), Times.Once);
        _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithNonExistingMovie_ThrowsNotFoundException()
    {
        var id = Guid.NewGuid();
        _repositoryMock
            .Setup(r => r.GetMovieByIdAsync(id))
            .ReturnsAsync((Movie?)null);

        var act = async () => await _service.UpdateAsync(id, new UpdateMovieDto("Inception", null, null, false, null));

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task UpdateAsync_WithCircularReference_ThrowsDomainException()
    {
        var movie1 = CreateMovie("Inception");
        var movie2 = CreateMovie("Inception 2", nextMovieId: movie1.Id);

        _repositoryMock
            .Setup(r => r.GetMovieByIdAsync(movie1.Id))
            .ReturnsAsync(movie1);
        _repositoryMock
            .Setup(r => r.GetMovieByIdAsync(movie2.Id))
            .ReturnsAsync(movie2);

        // movie1 -> movie2 -> movie1 = circular reference
        var act = async () => await _service.UpdateAsync(
            movie1.Id,
            new UpdateMovieDto("Inception", null, movie2.Id, false, null));

        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("*circular*");
    }

    [Fact]
    public async Task UpdateAsync_WithValidData_SavesChanges()
    {
        var movie = CreateMovie("Inception");
        _repositoryMock
            .Setup(r => r.GetMovieByIdAsync(movie.Id))
            .ReturnsAsync(movie);
        _repositoryMock
            .Setup(r => r.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        await _service.UpdateAsync(movie.Id, new UpdateMovieDto("Inception 2", 9, null, false, null));

        _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task MarkWatchedAsync_WithExistingMovie_UpdatesIsWatched()
    {
        var movie = CreateMovie();
        _repositoryMock
            .Setup(r => r.GetMovieByIdAsync(movie.Id))
            .ReturnsAsync(movie);
        _repositoryMock
            .Setup(r => r.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        var result = await _service.MarkWatchedAsync(movie.Id, new MarkWatchedMovieDto(true));

        result.IsWatched.Should().BeTrue();
        _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task MarkWatchedAsync_WithNonExistingMovie_ThrowsNotFoundException()
    {
        var id = Guid.NewGuid();
        _repositoryMock
            .Setup(r => r.GetMovieByIdAsync(id))
            .ReturnsAsync((Movie?)null);

        var act = async () => await _service.MarkWatchedAsync(id, new MarkWatchedMovieDto(true));

        await act.Should().ThrowAsync<NotFoundException>();
    }
}
using MediaTracker.Domain.Entities;

namespace MediaTracker.Application.Interfaces;

public interface INextMovieLinker
{
    Task TryLinkAsync(Movie movie);
    Task<bool> WouldCreateCycleAsync(Guid movieId, Guid? nextMovieId);
}
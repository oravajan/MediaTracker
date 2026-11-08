namespace MediaTracker.Application.Interfaces;

public interface IMediaUpdater
{
    Task UpdateAsync(Guid id);
}
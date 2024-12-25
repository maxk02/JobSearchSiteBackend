using Core.Domains._Shared.Entities;

namespace Core.Domains._Shared.JobSchedulers;

public interface IDeleteEntityScheduler<TEntity> where TEntity : BaseEntity
{
    public Task ScheduleDeletionAsync(long entityId);
}
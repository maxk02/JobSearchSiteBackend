using Domain.Shared.Entities.Interfaces;

namespace Domain.Shared.Repositories;

public interface IHierarchicalRepository<T> where T : class, IHierarchicalEntity<T>
{
    public Task<bool> IsParentOfAsync(long parentId, long childId, CancellationToken cancellationToken);
}
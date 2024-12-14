using Core._Shared.Entities;
using Core._Shared.Entities.Interfaces;

namespace Core._Shared.Repositories;

public interface IHierarchicalRepository<T> : IRepository<T> where T : BaseEntity, IHierarchicalEntity<T>
{
    public Task<bool> AreAncestorAndDescendantAsync(long ancestorId, long descendantId,
        CancellationToken cancellationToken);
}
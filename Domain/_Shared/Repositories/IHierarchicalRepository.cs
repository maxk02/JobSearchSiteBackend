using Domain._Shared.Entities;
using Domain._Shared.Entities.Interfaces;

namespace Domain._Shared.Repositories;

public interface IHierarchicalRepository<T> : IRepository<T> where T : BaseEntity, IHierarchicalEntity<T>
{
    public Task<bool> AreAncestorAndDescendantAsync(long ancestorId, long descendantId,
        CancellationToken cancellationToken);
}
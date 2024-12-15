using Core.Domains._Shared.Entities;
using Core.Domains._Shared.Entities.Interfaces;

namespace Core.Domains._Shared.Repositories;

public interface IHierarchicalRepository<T> : IRepository<T> where T : BaseEntity, IHierarchicalEntity<T>
{
    public Task<bool> AreAncestorAndDescendantAsync(long ancestorId, long descendantId,
        CancellationToken cancellationToken);
}
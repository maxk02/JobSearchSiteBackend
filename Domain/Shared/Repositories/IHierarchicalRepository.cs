using Domain.Shared.Entities;
using Domain.Shared.Entities.Interfaces;

namespace Domain.Shared.Repositories;

public interface IHierarchicalRepository<T> : IRepository<T> where T : BaseEntity, IHierarchicalEntity<T>
{
    public Task<bool> AreAncestorAndDescendantAsync(long ancestorId, long descendantId, CancellationToken cancellationToken);
}
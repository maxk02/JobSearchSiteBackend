using API.Domains._Shared.EntityInterfaces;

namespace API.Domains._Shared.Repositories;

public interface IHierarchicalRepository<T> : IRepository<T> where T : IEntityWithId, IHierarchicalEntity<T>
{
    public Task<bool> AreAncestorAndDescendantAsync(long ancestorId, long descendantId,
        CancellationToken cancellationToken);
}
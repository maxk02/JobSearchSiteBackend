using Core.Domains._Shared.Entities;
using Core.Domains._Shared.Entities.Interfaces;

namespace Core.Domains._Shared.Repositories;

public interface IHierarchicalRepository<T> where T : EntityBase, IHierarchicalEntity<T>
{
    public Task AddAsync(long parentId, T entity, CancellationToken cancellationToken = default);
    public Task RemoveAsync(T entity, CancellationToken cancellationToken = default);
    public Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

    public Task<ICollection<T>> GetDescendantsAsync(long id, int? depth = null,
        CancellationToken cancellationToken = default);

    public Task<ICollection<T>> GetAncestorsAsync(long id, CancellationToken cancellationToken = default);

    public Task<bool> AreAncestorAndDescendantAsync(long ancestorId, long descendantId,
        CancellationToken cancellationToken = default);
}
using Domain._Shared.Entities;

namespace Domain._Shared.Repositories;

public interface IRepository<T> where T : BaseEntity
{
    public Task<T?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    public Task<ICollection<T>> AddRangeAsync(ICollection<T> entities, CancellationToken cancellationToken = default);
    public Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    public Task UpdateRangeAsync(ICollection<T> entities, CancellationToken cancellationToken = default);
    public Task RemoveAsync(T entity, CancellationToken cancellationToken = default);
    public Task RemoveRangeAsync(ICollection<T> entities, CancellationToken cancellationToken = default);
}
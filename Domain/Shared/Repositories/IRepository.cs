using Domain.Shared.Entities;

namespace Domain.Shared.Repositories;

public interface IRepository<T> where T : BaseEntity
{
    public Task<T?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    // public Task<T?> FirstOrDefaultAsync(SingleResultSpecification<T>? specification = null, CancellationToken cancellationToken = default);
    // public Task<bool> AnyAsync(SingleResultSpecification<T> specification = null, CancellationToken cancellationToken = default);
    // public Task<List<T>> ListAsync(MultiResultSpecification<T>? specification = null, CancellationToken cancellationToken = default);
    // public Task<int> CountAsync(MultiResultSpecification<T>? specification = null, CancellationToken cancellationToken = default);
    
    public Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    public Task<IEnumerable<T>> AddRangeAsync(ICollection<T> entities, CancellationToken cancellationToken = default);
    public Task UpdateAsync(T entity);
    public Task UpdateRangeAsync(ICollection<T> entities);
    public Task RemoveAsync(T entity);
    public Task RemoveRangeAsync(ICollection<T> entities);
}
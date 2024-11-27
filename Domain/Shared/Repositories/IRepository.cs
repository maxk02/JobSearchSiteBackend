using Domain.Shared.Specifications;

namespace Domain.Shared.Repositories;

public interface IRepository<T> where T : class
{
    public Task<T?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<T?> FirstOrDefaultAsync(SingleResultSpecificationBase<T>? specification = null, CancellationToken cancellationToken = default);
    public Task<List<T>> ListAsync(MultiResultSpecificationBase<T>? specification = null, CancellationToken cancellationToken = default);
    public Task<int> CountAsync(MultiResultSpecificationBase<T>? specification = null, CancellationToken cancellationToken = default);
    
    public Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    public Task<IEnumerable<T>> AddRangeAsync(ICollection<T> entities, CancellationToken cancellationToken = default);
    public void Update(T entity);
    public void UpdateRange(ICollection<T> entities);
    public void Remove(T entity);
    public void RemoveRange(ICollection<T> entities);
}
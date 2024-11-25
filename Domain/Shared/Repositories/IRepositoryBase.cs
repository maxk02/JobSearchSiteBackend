namespace Domain.Shared.Repositories;

public interface IRepositoryBase<T>
{
    public Task<T?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<T?> FirstOrDefaultAsync(SingleSpecificationBase specification, CancellationToken cancellationToken = default);
    public Task<T?> SingleOrDefaultAsync(SingleSpecificationBase specification, CancellationToken cancellationToken = default);
    public Task<List<T>> ListAsync(MultiSpecificationBase? specification, CancellationToken cancellationToken = default);
    public Task<long> CountAsync(MultiSpecificationBase? specification, CancellationToken cancellationToken = default);
    
    public Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    public Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    public Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    public Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    public Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
    
}
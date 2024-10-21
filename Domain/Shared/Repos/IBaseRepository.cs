using Domain.Shared.Entities;

namespace Domain.Shared.Repos;

public interface IBaseRepository<T> where T : BaseEntity
{
    // Task<long> CreateAsync(T entity);
    // Task UpdateAsync(T entity);
    // Task DeleteAsync(long id);
    Task<T> GetByIdAsync(long id, CancellationToken cancellationToken);
}
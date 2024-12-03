using Domain.Shared.Entities;
using Domain.Shared.Repositories;
using Infrastructure.Persistence.EfCore.Context;

namespace Infrastructure.Persistence.EfCore.Repositories.Shared;

public class RepositoryBase<T> : IRepository<T>
    where T : BaseEntity
{
    protected readonly MyEfCoreDataContext DataDbContext;

    public RepositoryBase(MyEfCoreDataContext dataDbContext)
    {
        DataDbContext = dataDbContext;
    }
    
    public async Task<T?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await DataDbContext.Set<T>().FindAsync([id], cancellationToken: cancellationToken);
    }

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await DataDbContext.Set<T>().AddAsync(entity, cancellationToken: cancellationToken);

        return entity;
    }

    public async Task<IEnumerable<T>> AddRangeAsync(ICollection<T> entities, CancellationToken cancellationToken = default)
    {
        await DataDbContext.Set<T>().AddRangeAsync(entities, cancellationToken: cancellationToken);

        return entities;
    }

    public async Task UpdateAsync(T entity)
    {
        DataDbContext.Set<T>().Update(entity);
        await DataDbContext.SaveChangesAsync();
    }

    public async Task UpdateRangeAsync(ICollection<T> entities)
    {
        DataDbContext.Set<T>().UpdateRange(entities);
        await DataDbContext.SaveChangesAsync();
    }

    public async Task RemoveAsync(T entity)
    {
        DataDbContext.Set<T>().Remove(entity);
        await DataDbContext.SaveChangesAsync();
    }

    public async Task RemoveRangeAsync(ICollection<T> entities)
    {
        DataDbContext.Set<T>().RemoveRange(entities);
        await DataDbContext.SaveChangesAsync();
    }
}
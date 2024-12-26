using Core.Domains._Shared.Entities;
using Core.Domains._Shared.Repositories;
using Infrastructure.Persistence.EfCore.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.EfCore.Repositories._Shared;

public class RepositoryBase<T> : IRepository<T> where T : BaseEntity
{
    protected readonly MyEfCoreDataContext DataDbContext;

    public RepositoryBase(MyEfCoreDataContext dataDbContext)
    {
        DataDbContext = dataDbContext;
    }

    public async Task<bool> ExistsWithIdAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await DataDbContext.Set<T>().AnyAsync(x => x.Id == id, cancellationToken);
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task<T?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var value = await DataDbContext.Set<T>().FindAsync([id], cancellationToken: cancellationToken);

            return value;
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task<ICollection<T>> GetByIdsAsync(ICollection<long> ids,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var value = await DataDbContext.Set<T>().Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);

            return value;
        }
        catch
        {
            throw;
        }
    }

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        try
        {
            await DataDbContext.Set<T>().AddAsync(entity, cancellationToken: cancellationToken);
            await DataDbContext.SaveChangesAsync(cancellationToken);

            return entity;
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task<ICollection<T>> AddRangeAsync(ICollection<T> entities,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await DataDbContext.Set<T>().AddRangeAsync(entities, cancellationToken: cancellationToken);
            await DataDbContext.SaveChangesAsync(cancellationToken);

            return entities;
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        try
        {
            DataDbContext.Set<T>().Update(entity);
            await DataDbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task UpdateRangeAsync(ICollection<T> entities, CancellationToken cancellationToken = default)
    {
        try
        {
            DataDbContext.Set<T>().UpdateRange(entities);
            await DataDbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task RemoveAsync(T entity, CancellationToken cancellationToken = default)
    {
        try
        {
            DataDbContext.Set<T>().Remove(entity);
            await DataDbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task RemoveByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            await DataDbContext.Set<T>().Where(x => x.Id == id).ExecuteDeleteAsync(cancellationToken);
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task RemoveRangeAsync(ICollection<T> entities, CancellationToken cancellationToken = default)
    {
        try
        {
            DataDbContext.Set<T>().RemoveRange(entities);
            await DataDbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task RemoveRangeByIdsAsync(ICollection<long> ids, CancellationToken cancellationToken = default)
    {
        try
        {
            await DataDbContext.Set<T>().Where(x => ids.Contains(x.Id)).ExecuteDeleteAsync(cancellationToken);
        }
        catch (Exception e)
        {
            throw;
        }
    }
}
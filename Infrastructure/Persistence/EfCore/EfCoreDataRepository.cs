using Domain.Shared.Entities;
using Domain.Shared.Repositories;
using Domain.Shared.Specifications;
using Infrastructure.Persistence.EfCore.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.EfCore;

public class EfCoreDataRepository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly MyEfCoreDataContext DataDbContext;

    public EfCoreDataRepository(MyEfCoreDataContext dataDbContext)
    {
        DataDbContext = dataDbContext;
    }

    public async Task<T?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await DataDbContext.Set<T>().FindAsync([id], cancellationToken: cancellationToken);
    }

    public async Task<T?> FirstOrDefaultAsync(SingleResultSpecificationBase<T>? specification = null,
        CancellationToken cancellationToken = default)
    {
        return await ApplySingleResultSpecification(specification).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<T>> ListAsync(MultiResultSpecificationBase<T>? specification = null,
        CancellationToken cancellationToken = default)
    {
        return await ApplyMultiResultSpecification(specification).ToListAsync(cancellationToken);
    }

    public async Task<int> CountAsync(MultiResultSpecificationBase<T>? specification = null,
        CancellationToken cancellationToken = default)
    {
        return await ApplyMultiResultSpecification(specification).CountAsync(cancellationToken);
    }

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await DataDbContext.Set<T>().AddAsync(entity, cancellationToken: cancellationToken);

        return entity;
    }

    public async Task<IEnumerable<T>> AddRangeAsync(ICollection<T> entities,
        CancellationToken cancellationToken = default)
    {
        await DataDbContext.Set<T>().AddRangeAsync(entities, cancellationToken: cancellationToken);

        return entities;
    }

    public void Update(T entity)
    {
        DataDbContext.Set<T>().Update(entity);
    }

    public void UpdateRange(ICollection<T> entities)
    {
        DataDbContext.Set<T>().UpdateRange(entities);
    }

    public void Remove(T entity)
    {
        DataDbContext.Set<T>().Remove(entity);
    }

    public void RemoveRange(ICollection<T> entities)
    {
        DataDbContext.Set<T>().RemoveRange(entities);
    }

    protected IQueryable<T> ApplySingleResultSpecification(SingleResultSpecificationBase<T>? specification)
    {
        var query = DataDbContext.Set<T>().AsQueryable();

        if (specification is null)
            return query;

        if (specification.Criteria != null)
        {
            query = query.Where(specification.Criteria);
        }

        if (specification.OrderBy.Any())
        {
            // Apply the first OrderBy
            var firstOrderBy = specification.OrderBy[0];
            query = firstOrderBy.Ascending 
                ? query.OrderBy(firstOrderBy.Expression) 
                : query.OrderByDescending(firstOrderBy.Expression);

            // Apply the remaining ThenBy or ThenByDescending
            query = specification.OrderBy.Skip(1)
                .Aggregate(query, (current, orderBySpec) => orderBySpec.Ascending
                    ? ((IOrderedQueryable<T>)current).ThenBy(orderBySpec.Expression)
                    : ((IOrderedQueryable<T>)current).ThenByDescending(orderBySpec.Expression));
        }

        query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));

        return query;
    }

    protected IQueryable<T> ApplyMultiResultSpecification(MultiResultSpecificationBase<T>? specification)
    {
        var query = ApplySingleResultSpecification(specification);

        if (specification is null)
            return query;

        if (specification.Take != null && specification.Skip != null)
        {
            query = query.Skip(specification.Skip.Value).Take(specification.Take.Value);
        }

        return query;
    }
}
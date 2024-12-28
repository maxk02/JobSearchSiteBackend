using Core.Domains._Shared.Entities;
using Core.Domains._Shared.Entities.Interfaces;
using Core.Domains._Shared.Repositories;
using Infrastructure.Persistence.EfCore.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.EfCore.Repositories._Shared;

public class HierarchicalRepositoryBase<T> : IHierarchicalRepository<T>
    where T : EntityBase, IHierarchicalEntity<T>
{
    protected readonly MyEfCoreDataContext DataDbContext;

    public HierarchicalRepositoryBase(MyEfCoreDataContext dataDbContext)
    {
        DataDbContext = dataDbContext;
    }

    public async Task AddAsync(long parentId, T entity, CancellationToken cancellationToken = default)
    {
        using var transaction = await DataDbContext.Database.BeginTransactionAsync(cancellationToken);

        // Add the new node
        DataDbContext.Set<T>().Add(entity);
        await DataDbContext.SaveChangesAsync(cancellationToken);

        // Add Closure relationships
        var parentClosures = await DataDbContext.Set<Closure<T>>()
            .Where(c => c.DescendantId == parentId)
            .ToListAsync(cancellationToken);

        var newClosures = parentClosures.Select(parentClosure => new Closure<T>
            (parentClosure.AncestorId, entity.Id, parentClosure.Depth + 1)
        ).ToList();

        // Add self-relationship for the new node
        newClosures.Add(new Closure<T>(entity.Id, entity.Id, 0));

        DataDbContext.Set<Closure<T>>().AddRange(newClosures);
        await DataDbContext.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);
    }

    public async Task RemoveAsync(T entity, CancellationToken cancellationToken = default)
    {
        DataDbContext.Set<T>().Remove(entity);
        await DataDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        DataDbContext.Set<T>().Update(entity);
        await DataDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<ICollection<T>> GetDescendantsAsync(long id, int? depth = null,
        CancellationToken cancellationToken = default)
    {
        var query = DataDbContext.Set<Closure<T>>()
            .Include(c => c.Descendant)
            .Where(c => c.AncestorId == id);

        if (depth.HasValue)
        {
            query = query.Where(c => c.Depth <= depth.Value);
        }

        return await query.Select(c => c.Descendant!).ToListAsync(cancellationToken);
    }

    public async Task<ICollection<T>> GetAncestorsAsync(long id, CancellationToken cancellationToken = default)
    {
        return await DataDbContext.Set<Closure<T>>()
            .Include(c => c.Ancestor)
            .Where(c => c.DescendantId == id && c.Depth > 0)
            .Select(c => c.Ancestor!)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> AreAncestorAndDescendantAsync(long ancestorId, long descendantId,
        CancellationToken cancellationToken = default)
    {
        return await DataDbContext.Set<Closure<T>>()
            .AnyAsync(c => c.AncestorId == ancestorId && c.DescendantId == descendantId, cancellationToken);
    }
}
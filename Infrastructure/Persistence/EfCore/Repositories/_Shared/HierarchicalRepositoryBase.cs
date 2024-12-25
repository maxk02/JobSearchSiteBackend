using Core.Domains._Shared.Entities;
using Core.Domains._Shared.Entities.Interfaces;
using Core.Domains._Shared.Repositories;
using Infrastructure.Persistence.EfCore.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.EfCore.Repositories._Shared;

public class HierarchicalRepositoryBase<T> : RepositoryBase<T>, IHierarchicalRepository<T>
    where T : BaseEntity, IHierarchicalEntity<T>
{
    public HierarchicalRepositoryBase(MyEfCoreDataContext dataDbContext) : base(dataDbContext)
    {
    }

    public async Task<bool> AreAncestorAndDescendantAsync(long ancestorId, long descendantId, CancellationToken cancellationToken)
    {
        var tableName = DataDbContext.Model
            .FindEntityType(typeof(T))?
            .GetTableName();

        if (string.IsNullOrEmpty(tableName))
        {
            throw new Exception($"The table name {tableName} could not be found.");
        }

        try
        {
            var sql = $@"
            WITH RecursiveHierarchy AS (
                SELECT *
                FROM {tableName}
                WHERE Id = {{0}}
                UNION ALL
                SELECT *
                FROM {tableName} e
                INNER JOIN RecursiveHierarchy rh ON e.ParentId = rh.Id
            )
            SELECT *
            FROM RecursiveHierarchy
            WHERE Id = {{1}};
        ";

            var result = await DataDbContext
                .Set<T>() //todo
                .FromSqlRaw(sql, ancestorId, descendantId)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return result.Any();
        }
        catch (Exception e)
        {
            throw;
        }
    }
}
using Domain.Shared.Entities.Interfaces;
using Domain.Shared.Repositories;
using Infrastructure.Persistence.EfCore.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.EfCore;

public class EfCoreHierarchicalDataRepository<T> : IHierarchicalRepository<T> where T : class, IHierarchicalEntity<T>
{
    protected readonly MyEfCoreDataContext DataDbContext;

    public EfCoreHierarchicalDataRepository(MyEfCoreDataContext dataDbContext)
    {
        DataDbContext = dataDbContext;
    }
    
    public async Task<bool> IsParentOfAsync(long parentId, long childId, CancellationToken cancellationToken = default)
    {
        var tableName = DataDbContext.Model
            .FindEntityType(typeof(T))?
            .GetTableName();

        if (string.IsNullOrEmpty(tableName))
            throw new NullReferenceException();

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
            .FromSqlRaw(sql, parentId, childId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return result.Any();
    }
}
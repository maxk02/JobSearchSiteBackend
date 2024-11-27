using Domain.Shared.Repositories;
using Infrastructure.Persistence.EfCore.Context;

namespace Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    protected readonly MyEfCoreDataContext DataDbContext;

    public UnitOfWork(MyEfCoreDataContext dataDbContext)
    {
        DataDbContext = dataDbContext;
    }
    
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await DataDbContext.SaveChangesAsync(cancellationToken);
    }
}
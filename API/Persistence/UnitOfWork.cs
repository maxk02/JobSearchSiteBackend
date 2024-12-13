using API.Domains._Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace API.Persistence.EfCore;

public abstract class UnitOfWork<TContext> : IUnitOfWork, IAsyncDisposable where TContext : DbContext
{
    private readonly TContext _dataDbContext;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(TContext dataDbContext)
    {
        _dataDbContext = dataDbContext;
    }

    public async Task BeginAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            throw new InvalidOperationException("Cannot begin, transaction already started.");
        }
        
        _transaction = await _dataDbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
        {
            throw new InvalidOperationException("Cannot rollback, transaction not started.");
        }

        try
        {
            await _transaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            _transaction.Dispose();
            _transaction = null;
        }
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
        {
            throw new InvalidOperationException("Cannot commit, transaction not started.");
        }
        
        try
        {
            await _transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await RollbackAsync(cancellationToken);
            throw;
        }
        finally
        {
            _transaction?.Dispose();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _dataDbContext.Dispose();
        _transaction?.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _dataDbContext.DisposeAsync();
        if (_transaction != null) await _transaction.DisposeAsync();
    }
}
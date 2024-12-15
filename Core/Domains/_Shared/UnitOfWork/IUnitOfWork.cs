namespace Core.Domains._Shared.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    public Task BeginAsync(CancellationToken cancellationToken = default);
    public Task RollbackAsync(CancellationToken cancellationToken = default);
    public Task CommitAsync(CancellationToken cancellationToken = default);
}
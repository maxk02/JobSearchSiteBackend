namespace Domain._Shared.Repositories;

public interface IUnitOfWork : IDisposable
{
    public Task BeginAsync(CancellationToken cancellationToken = default);
    public Task RollbackAsync(CancellationToken cancellationToken = default);
    public Task CommitAsync(CancellationToken cancellationToken = default);
}
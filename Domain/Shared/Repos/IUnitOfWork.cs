namespace Domain.Shared.Repos;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
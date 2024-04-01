namespace Application.Repositories.Persistence.Common;

public interface IUnitOfWork
{
    Task Save(CancellationToken cancellationToken);
}
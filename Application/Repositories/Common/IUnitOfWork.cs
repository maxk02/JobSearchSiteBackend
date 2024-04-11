namespace Application.Repositories.Common;

public interface IUnitOfWork
{
    Task Save(CancellationToken cancellationToken);
}
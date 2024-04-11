using Domain.Common;

namespace Application.Repositories.Common;

public interface IBaseRepository<T> where T : BaseEntity
{
    void Create(T entity);
    void Update(T entity);
    void Delete(long id);
    Task<T> GetById(long id, CancellationToken cancellationToken);
    // Task<IList<T>> GetAll(CancellationToken cancellationToken);
}
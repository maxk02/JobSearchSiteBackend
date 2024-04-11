using Application.Repositories.Common;
using Domain.Entities;

namespace Application.Repositories;

public interface ICountryRepository : IBaseRepository<Country>
{
    Task<IList<Country>> GetAll(CancellationToken cancellationToken);
}
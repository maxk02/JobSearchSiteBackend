using Application.Repositories.Common;
using Domain.Entities;

namespace Application.Repositories;

public interface ILocationRepository : IBaseRepository<Location>
{
    Task<IList<Location>> GetByCountryId(long countryId, int resultLimit);
}
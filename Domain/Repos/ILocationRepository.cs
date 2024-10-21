using Domain.Entities;
using Domain.Entities.Locations;
using Domain.Shared.Repos;

namespace Domain.Repos;

public interface ILocationRepository : IBaseRepository<Location>
{
    Task<IList<Location>> GetByCountryId(long countryId, int resultLimit);
}
using Core.Domains._Shared.Search;

namespace Core.Domains.Locations.Search;

public interface ILocationSearchRepository : ISearchRepository<LocationSearchModel>
{
    Task<IList<long>> SearchByCountryId(string query, long countryId);
}
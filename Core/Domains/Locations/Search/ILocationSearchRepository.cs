using Core.Domains._Shared.Search;

namespace Core.Domains.Locations.Search;

public interface ILocationSearchRepository : ISearchRepository<LocationSearchModel>
{
    Task<ICollection<long>> SearchByCountryIdAsync(long countryId, string query,
        CancellationToken cancellationToken = default);
}
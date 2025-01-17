using Core.Domains._Shared.Search;

namespace Core.Domains.Locations.Search;

public interface ILocationSearchRepository : ISearchRepository<LocationSearchModel>,
    IBulkSearchRepository<LocationSearchModel>
{
    Task<ICollection<long>> SearchFromCountryIdAsync(long countryId, string query,
        CancellationToken cancellationToken = default);
}
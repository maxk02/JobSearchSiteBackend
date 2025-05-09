using JobSearchSiteBackend.Core.Services.Search;

namespace JobSearchSiteBackend.Core.Domains.Locations.Search;

public interface ILocationSearchRepository : ISearchRepository<LocationSearchModel>
{
    Task<ICollection<long>> SearchFromCountryIdAsync(long countryId, string query,
        CancellationToken cancellationToken = default);
}
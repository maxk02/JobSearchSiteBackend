using JobSearchSiteBackend.Core.Services.Search;

namespace JobSearchSiteBackend.Core.Domains.Locations.Search;

public interface ILocationSearchRepository : ISearchRepository<LocationSearchModel>
{
    Task<ICollection<LocationSearchModel>> SearchFromCountryIdAsync(long countryId, string query, int size,
        CancellationToken cancellationToken = default);
}
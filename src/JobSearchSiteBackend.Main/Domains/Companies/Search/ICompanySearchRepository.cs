using JobSearchSiteBackend.Main.Services.Search;

namespace JobSearchSiteBackend.Main.Domains.Companies.Search;

public interface ICompanySearchRepository : ISearchRepository<CompanySearchModel>,
    IUpdatableSearchRepository<CompanySearchModel>
{
    public Task<ICollection<long>> SearchFromCountryIdAsync(long countryId, string query,
        CancellationToken cancellationToken = default);
}
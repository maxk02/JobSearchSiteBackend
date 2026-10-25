using JobSearchSiteBackend.Main.Services.Search;

namespace JobSearchSiteBackend.Main.Domains.Jobs.Search;

public interface IJobSearchRepository : ISearchRepository<JobSearchModel>, IUpdatableSearchRepository<JobSearchModel>
{
    Task<ICollection<long>> SearchFromCountriesAndCategoriesAsync(ICollection<long> countryIds,
        ICollection<long> categoryIds, string query, CancellationToken cancellationToken = default);
    Task<ICollection<long>> SearchFromCompanyAsync(long companyId, string query,
        CancellationToken cancellationToken = default);
}
using Core.Domains._Shared.Search;

namespace Core.Domains.Companies.Search;

public interface ICompanySearchRepository : ISearchRepository<CompanySearchModel>,
    IConcurrentSearchRepository<CompanySearchModel>
{
    public Task<ICollection<long>> SearchFromCountryIdAsync(long countryId, string query,
        CancellationToken cancellationToken = default);
}
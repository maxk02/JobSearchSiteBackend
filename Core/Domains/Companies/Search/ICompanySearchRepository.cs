using Core.Domains._Shared.Search;

namespace Core.Domains.Companies.Search;

public interface ICompanySearchRepository : ISearchRepository<CompanySearchModel>
{
    public Task<ICollection<long>> SearchByCountryIdAsync(long countryId, string query,
        CancellationToken cancellationToken = default);
}
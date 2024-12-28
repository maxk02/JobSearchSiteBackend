using Core.Domains._Shared.Search;

namespace Core.Domains.Companies.Search;

public interface ICompanySearchRepository : ISearchRepository<CompanySearchModel>
{
    public Task<IList<long>> SearchByCountryIdAsync(string query, long countryId);
}
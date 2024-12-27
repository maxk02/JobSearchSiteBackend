using Core.Domains._Shared.Search;

namespace Core.Domains.Jobs.Search;

public interface IJobSearchRepository : ISearchRepository<JobSearchModel>
{
    Task<IList<int>> SearchForCompanyId(string query, int companyId);
}
using Core._Shared.Services.Search.Common;

namespace Core._Shared.Services.Search.Job;

public interface IJobSearchService : IBaseSearchService<JobSearchModel>
{
    Task<IList<int>> SearchForCompanyId(string query, int companyId);
}
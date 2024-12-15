using Core.Services.Search.Common;

namespace Core.Services.Search.Job;

public interface IJobSearchService : IBaseSearchService<JobSearchModel>
{
    Task<IList<int>> SearchForCompanyId(string query, int companyId);
}
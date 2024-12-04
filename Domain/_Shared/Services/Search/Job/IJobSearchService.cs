using Domain._Shared.Services.Search.Common;

namespace Domain._Shared.Services.Search.Job;

public interface IJobSearchService : IBaseSearchService<JobSearchModel>
{
    Task<IList<int>> SearchForCompanyId(string query, int companyId);
}
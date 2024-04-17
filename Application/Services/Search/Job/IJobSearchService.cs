using Application.Services.Search.Common;

namespace Application.Services.Search.Job;

public interface IJobSearchService : IBaseSearchService<JobSearchModel>
{
    Task<IList<int>> SearchForCompanyId(string query, int companyId);
}
using Application.Services.Search.Common;

namespace Application.Services.Search.Job;

public interface IJobSearchService : IBaseSearchService<JobSearchModel>
{
    Task<List<long>> SearchForCompanyId(string query, long companyId);
}
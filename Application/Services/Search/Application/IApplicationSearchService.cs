using Application.Services.Search.Common;

namespace Application.Services.Search.Application;

public interface IApplicationSearchService : IBaseSearchService<ApplicationSearchModel>
{
    Task<List<long>> SearchForJobId(string query, long jobId);
}
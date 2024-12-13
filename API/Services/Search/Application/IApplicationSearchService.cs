using API.Services.Search.Common;

namespace API.Services.Search.Application;

public interface IApplicationSearchService : IBaseSearchService<ApplicationSearchModel>
{
    Task<IList<int>> SearchForJobId(string query, int jobId);
}
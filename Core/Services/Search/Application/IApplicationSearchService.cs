using Core.Services.Search.Common;

namespace Core.Services.Search.Application;

public interface IApplicationSearchService : IBaseSearchService<ApplicationSearchModel>
{
    Task<IList<int>> SearchForJobId(string query, int jobId);
}
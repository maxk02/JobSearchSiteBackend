using Core._Shared.Services.Search.Common;

namespace Core._Shared.Services.Search.Application;

public interface IApplicationSearchService : IBaseSearchService<ApplicationSearchModel>
{
    Task<IList<int>> SearchForJobId(string query, int jobId);
}
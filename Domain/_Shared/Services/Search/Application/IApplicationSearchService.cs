using Domain._Shared.Services.Search.Common;

namespace Domain._Shared.Services.Search.Application;

public interface IApplicationSearchService : IBaseSearchService<ApplicationSearchModel>
{
    Task<IList<int>> SearchForJobId(string query, int jobId);
}
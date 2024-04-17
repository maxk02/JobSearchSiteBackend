using Application.Services.Search.Common;

namespace Application.Services.Search.Application;

public interface IApplicationSearchService : IBaseSearchService<ApplicationSearchModel>
{
    Task<IList<int>> SearchForJobId(string query, int jobId);
}
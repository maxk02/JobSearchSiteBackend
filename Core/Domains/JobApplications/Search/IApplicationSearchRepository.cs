using Core.Domains._Shared.Search;

namespace Core.Domains.JobApplications.Search;

public interface IApplicationSearchRepository : ISearchRepository<ApplicationSearchModel>
{
    Task<IList<int>> SearchForJobId(string query, int jobId);
}
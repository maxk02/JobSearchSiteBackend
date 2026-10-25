using JobSearchSiteBackend.Main.Services.Search;

namespace JobSearchSiteBackend.Main.Domains.JobApplications.Search;

public interface IJobApplicationSearchRepository : ISearchRepository<JobApplicationSearchModel>,
    IUpdatableSearchRepository<JobApplicationSearchModel>
{
    Task<ICollection<long>> SearchFromJobAsync(long jobId, string query,
        CancellationToken cancellationToken = default);
}
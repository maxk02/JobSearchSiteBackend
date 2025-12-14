namespace JobSearchSiteBackend.Core.Domains.Jobs.RecurringJobRunners;

public interface ISyncJobsWithSearchRunner
{
    public Task Run();
}
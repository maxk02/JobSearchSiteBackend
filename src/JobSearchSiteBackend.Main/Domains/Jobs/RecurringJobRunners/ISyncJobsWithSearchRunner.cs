namespace JobSearchSiteBackend.Main.Domains.Jobs.RecurringJobRunners;

public interface ISyncJobsWithSearchRunner
{
    public Task Run();
}
namespace JobSearchSiteBackend.Main.Domains.JobApplications.RecurringJobRunners;

public interface ISyncJobApplicationsWithSearchRunner
{
    public Task Run();
}
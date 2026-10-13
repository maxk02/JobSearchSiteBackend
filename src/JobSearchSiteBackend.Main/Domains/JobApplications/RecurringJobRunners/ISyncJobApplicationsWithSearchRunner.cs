namespace JobSearchSiteBackend.Core.Domains.JobApplications.RecurringJobRunners;

public interface ISyncJobApplicationsWithSearchRunner
{
    public Task Run();
}
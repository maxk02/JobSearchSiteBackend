namespace JobSearchSiteBackend.Infrastructure.BackgroundJobs;

public interface IRecurringJobRegisterer
{
    public Task RegisterJobsAsync();
}
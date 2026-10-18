namespace JobSearchSiteBackend.Main.Services.BackgroundJobs;

public interface IRecurringJobRegisterer
{
    public Task RegisterJobsAsync();
}
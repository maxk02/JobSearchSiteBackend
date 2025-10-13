using JobSearchSiteBackend.Core.Domains.Companies.RecurringJobs;
using JobSearchSiteBackend.Core.Domains.Jobs.RecurringJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JobSearchSiteBackend.Core.Services.BackgroundJobs;

public static class ConfigureRecurringJobs
{
    public static void Configure(IBackgroundJobService backgroundJobService)
    {
        backgroundJobService.AddOrUpdateRecurring("sql-search-sync-jobs", 
            () => SyncJobDataWithSearchEngine.Run(), "*/2 * * * *");
        backgroundJobService.AddOrUpdateRecurring("sql-search-sync-companies", 
            () => SyncCompanyDataWithSearchEngine.Run(), "*/2 * * * *");
    }
}
using JobSearchSiteBackend.Core.Domains.Companies.Search;
using JobSearchSiteBackend.Core.Domains.Jobs.RecurringJobs;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.BackgroundJobs;

namespace JobSearchSiteBackend.Core.Domains.Companies.RecurringJobs;

public static class SyncCompaniesWithSearchRecurringJob
{
    public static async Task Register(MainDataContext dbContext,
        ICompanySearchRepository companySearchRepository,
        IBackgroundJobService backgroundJobService)
    {
        backgroundJobService.AddOrUpdateRecurring("sql-search-sync-companies", 
            () => Run(dbContext, companySearchRepository), "*/2 * * * *");
        
        await Task.CompletedTask;
    }
    
    public static async Task Run(MainDataContext dbContext, ICompanySearchRepository companySearchRepository)
    {
        
    }
}
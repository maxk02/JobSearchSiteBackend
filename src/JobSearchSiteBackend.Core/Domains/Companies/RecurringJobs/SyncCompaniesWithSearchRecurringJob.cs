using JobSearchSiteBackend.Core.Domains.Companies.Search;
using JobSearchSiteBackend.Core.Domains.Jobs.RecurringJobs;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.BackgroundJobs;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Companies.RecurringJobs;

public static class SyncCompaniesWithSearchRecurringJob
{
    public static readonly int SyncPeriodMinutes = 2;

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
        var recordsToSync = await dbContext.Companies
            .Where(c => c.DateTimeSyncedWithSearchUtc == null
                        || c.DateTimeSyncedWithSearchUtc < c.DateTimeUpdatedUtc)
            .ToListAsync();
        
        if (recordsToSync.Count == 0)
            return;
        
        var companySearchModels = recordsToSync
            .Select(c => new CompanySearchModel(
                c.Id,
                c.CountryId,
                c.Name,
                c.Description,
                c.DateTimeUpdatedUtc,
                c.IsDeleted
            ))
            .ToList();

        await companySearchRepository.UpsertMultipleAsync(companySearchModels);
        
        var dateTimeSyncedUtc = DateTime.UtcNow;

        foreach (var record in recordsToSync)
        {
            record.DateTimeSyncedWithSearchUtc = dateTimeSyncedUtc;
        }
        
        await dbContext.SaveChangesAsync();
    }
}
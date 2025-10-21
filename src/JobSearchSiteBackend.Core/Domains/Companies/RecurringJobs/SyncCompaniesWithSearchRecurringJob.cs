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
        var lastUpdateInfo = await dbContext
            .SqlToSearchSyncInfos
            .Where(x => x.CollectionName == "companies")
            .SingleOrDefaultAsync();

        if (lastUpdateInfo == null)
        {
            throw new InvalidDataException();
        }

        // var vs this type??
        IQueryable<Company> query = dbContext.Companies.AsNoTracking();

        if (lastUpdateInfo.UpdatedUpToDateTimeUtc != null)
        {
            query = query
                .Where(j => j.DateTimeUpdatedUtc > lastUpdateInfo.UpdatedUpToDateTimeUtc)
                .OrderBy(x => x.DateTimeUpdatedUtc);
        }

        var recordsToUpdate = await query.ToListAsync();

        var companySearchModels = recordsToUpdate.Select(c => new CompanySearchModel(
            c.Id,
            c.CountryId,
            c.Name,
            c.Description,
            c.DateTimeUpdatedUtc,
            c.IsDeleted
        )).ToList();

        await companySearchRepository.UpsertMultipleAsync(companySearchModels);
        
        lastUpdateInfo.UpdatedUpToDateTimeUtc = recordsToUpdate.Last().DateTimeUpdatedUtc;
        lastUpdateInfo.LastTimeSyncedUtc = DateTime.UtcNow;
        await dbContext.SaveChangesAsync();
    }
}
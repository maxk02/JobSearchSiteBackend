using JobSearchSiteBackend.Core.Domains.Jobs.Search;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.BackgroundJobs;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Jobs.RecurringJobs;

public static class SyncJobsWithSearchRecurringJob
{
    public static readonly int SyncPeriodMinutes = 2;
    
    public static async Task Register(MainDataContext dbContext,
        IJobSearchRepository jobSearchRepository,
        IBackgroundJobService backgroundJobService)
    {
        backgroundJobService.AddOrUpdateRecurring("sql-search-sync-jobs", 
            () => Run(dbContext, jobSearchRepository), $"*/{SyncPeriodMinutes} * * * *");
        
        await Task.CompletedTask;
    }
    
    public static async Task Run(MainDataContext dbContext, IJobSearchRepository jobSearchRepository)
    {
        var recordsToSync = await dbContext.Jobs
            .Include(j => j.JobFolder)
            .ThenInclude(jf => jf!.Company)
            .Where(j => j.DateTimeSyncedWithSearchUtc == null
                        || j.DateTimeSyncedWithSearchUtc < j.DateTimeUpdatedUtc)
            .ToListAsync();
        
        if (recordsToSync.Count == 0)
            return;
        
        var jobSearchModels = recordsToSync.Select(job => new JobSearchModel(
            job.Id,
            job.JobFolder!.Company!.CountryId,
            job.CategoryId,
            job.Title,
            job.Description,
            job.Responsibilities ?? [],
            job.Requirements ?? [],
            job.NiceToHaves ?? [],
            job.DateTimeUpdatedUtc,
            job.IsDeleted
        )).ToList();

        await jobSearchRepository.UpsertMultipleAsync(jobSearchModels);
        
        var dateTimeSyncedUtc = DateTime.UtcNow;
        
        foreach (var record in recordsToSync)
        {
            record.DateTimeSyncedWithSearchUtc = dateTimeSyncedUtc;
        }
        
        await dbContext.SaveChangesAsync();
    }
}
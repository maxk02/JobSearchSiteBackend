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
        var lastUpdateInfo = await dbContext
                .SqlToSearchSyncInfos
                .Where(x => x.CollectionName == "jobs")
                .SingleOrDefaultAsync();

        if (lastUpdateInfo == null)
        {
            throw new InvalidDataException();
        }

        // var vs this type??
        IQueryable<Job> query = dbContext.Jobs
            .AsNoTracking()
            .Include(j => j.JobFolder)
            .ThenInclude(jf => jf!.Company);
        
        if (lastUpdateInfo.UpdatedUpToDateTimeUtc != null)
        {
            query = query
                .Where(j => j.DateTimeUpdatedUtc > lastUpdateInfo.UpdatedUpToDateTimeUtc)
                .OrderBy(x => x.DateTimeUpdatedUtc);
        }

        var recordsToUpdate = await query.ToListAsync();
        
        var jobSearchModels = recordsToUpdate.Select(job => new JobSearchModel(
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
        
        lastUpdateInfo.UpdatedUpToDateTimeUtc = recordsToUpdate.Last().DateTimeUpdatedUtc;
        lastUpdateInfo.LastTimeSyncedUtc = DateTime.UtcNow;
        await dbContext.SaveChangesAsync();
    }
}
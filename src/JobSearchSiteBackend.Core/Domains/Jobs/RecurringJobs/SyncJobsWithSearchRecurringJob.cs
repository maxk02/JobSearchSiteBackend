using JobSearchSiteBackend.Core.Domains.Jobs.Search;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.BackgroundJobs;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Jobs.RecurringJobs;

public static class SyncJobsWithSearchRecurringJob
{
    public static async Task Register(MainDataContext dbContext,
        IJobSearchRepository jobSearchRepository,
        IBackgroundJobService backgroundJobService)
    {
        backgroundJobService.AddOrUpdateRecurring("sql-search-sync-jobs", 
            () => Run(dbContext, jobSearchRepository), "*/2 * * * *");
        
        await Task.CompletedTask;
    }
    
    public static async Task Run(MainDataContext dbContext, IJobSearchRepository jobSearchRepository)
    {
        var lastUpdateInfo = await dbContext
                .SqlToSearchSyncInfos
                .Where(x => x.CollectionName == "jobs")
                .SingleOrDefaultAsync();

        if (lastUpdateInfo == null || lastUpdateInfo.UpdatedUpToDateTimeUtc == null)
        {
            return;
        }

        var recordsToUpdate = await dbContext.Jobs
            .AsNoTracking()
            .Include(j => j.JobFolder)
            .ThenInclude(jf => jf!.Company)
            .Where(j => j.LastUpdatedUtc > lastUpdateInfo.UpdatedUpToDateTimeUtc)
            .ToListAsync();
        
        var jobSearchModels = recordsToUpdate.Select(job => new JobSearchModel(
            job.Id,
            job.JobFolder!.Company!.CountryId,
            job.CategoryId,
            job.Title,
            job.Description,
            job.Responsibilities ?? [],
            job.Requirements ?? [],
            job.NiceToHaves ?? []
        ));
        
        // await jobSearchRepository
        await Task.CompletedTask;
    }
}
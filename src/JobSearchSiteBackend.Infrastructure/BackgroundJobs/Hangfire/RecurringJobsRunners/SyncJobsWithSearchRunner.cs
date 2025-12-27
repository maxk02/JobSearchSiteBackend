using JobSearchSiteBackend.Core.Domains.Jobs.RecurringJobRunners;
using JobSearchSiteBackend.Core.Domains.Jobs.Search;
using JobSearchSiteBackend.Core.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Infrastructure.BackgroundJobs.Hangfire.RecurringJobsRunners;

public class SyncJobsWithSearchRunner(MainDataContext dbContext,
    IJobSearchRepository jobSearchRepository) : ISyncJobsWithSearchRunner
{
    public async Task Run()
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
        
        var idsToUpdate = recordsToSync.Select(x => x.Id).ToList();

        // EXECUTE UPDATE:
        // This generates a direct SQL UPDATE statement.
        // It bypasses the ChangeTracker, effectively skipping all SaveChanges interceptors.
        await dbContext.Jobs
            .Where(j => idsToUpdate.Contains(j.Id))
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(j => j.DateTimeSyncedWithSearchUtc, j => DateTime.UtcNow));
    }
}
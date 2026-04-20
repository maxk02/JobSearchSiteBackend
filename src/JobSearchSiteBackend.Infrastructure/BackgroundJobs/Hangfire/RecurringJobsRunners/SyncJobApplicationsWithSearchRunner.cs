using JobSearchSiteBackend.Core.Domains.JobApplications.RecurringJobRunners;
using JobSearchSiteBackend.Core.Domains.JobApplications.Search;
using JobSearchSiteBackend.Core.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Infrastructure.BackgroundJobs.Hangfire.RecurringJobsRunners;

public class SyncJobApplicationsWithSearchRunner(MainDataContext dbContext,
    IJobApplicationSearchRepository jobApplicationSearchRepository) : ISyncJobApplicationsWithSearchRunner
{
    public async Task Run()
    {
        var recordsToSyncQuery = dbContext.JobApplications
            .Where(ja => ja.VersionIdSyncedWithSearch == null
                         || ja.VersionId != ja.VersionIdSyncedWithSearch);
        
        var count = await recordsToSyncQuery.CountAsync();
        
        if (count == 0)
            return;

        var jobApplicationSearchModels = await recordsToSyncQuery
            .Select(jobApplication => new JobApplicationSearchModel(
                jobApplication.Id,
                jobApplication.JobId,
                jobApplication.PersonalFiles!.Select(pf => pf.Text).ToList(),
                jobApplication.VersionId,
                jobApplication.IsDeleted
            ))
            .ToListAsync();

        await jobApplicationSearchRepository.UpsertMultipleAsync(jobApplicationSearchModels);

        var idsToUpdate = jobApplicationSearchModels.Select(x => x.Id).ToList();

        // EXECUTE UPDATE:
        // This generates a direct SQL UPDATE statement.
        // It bypasses the ChangeTracker, effectively skipping all SaveChanges interceptors.
        // await dbContext.JobApplications
        //     .Where(ja => idsToUpdate.Contains(ja.Id))
        //     .ExecuteUpdateAsync(setters => setters
        //         .SetProperty(ja => ja.DateTimeSyncedWithSearchUtc, pf => DateTime.UtcNow));
        
        await dbContext.JobApplications
            .Where(ja => idsToUpdate.Contains(ja.Id))
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(ja => ja.VersionIdSyncedWithSearch, ja => ja.VersionId));
    }
}
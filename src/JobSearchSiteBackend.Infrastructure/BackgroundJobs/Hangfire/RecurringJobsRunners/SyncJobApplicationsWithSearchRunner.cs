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
        var recordsToSync = await dbContext.JobApplications
            .Include(jobApplication => jobApplication.PersonalFiles)
            .Where(pf => pf.DateTimeSyncedWithSearchUtc == null
                         || pf.DateTimeSyncedWithSearchUtc < pf.DateTimeUpdatedUtc)
            .ToListAsync();
        
        if (recordsToSync.Count == 0)
            return;

        var personalFileSearchModels = recordsToSync
            .Select(jobApplication => new JobApplicationSearchModel(
                jobApplication.Id,
                jobApplication.JobId,
                jobApplication.PersonalFiles!.Select(pf => pf.Text).ToList(),
                jobApplication.DateTimeUpdatedUtc,
                jobApplication.IsDeleted
            ))
            .ToList();

        await jobApplicationSearchRepository.UpsertMultipleAsync(personalFileSearchModels);

        var idsToUpdate = recordsToSync.Select(x => x.Id).ToList();

        // EXECUTE UPDATE:
        // This generates a direct SQL UPDATE statement.
        // It bypasses the ChangeTracker, effectively skipping all SaveChanges interceptors.
        await dbContext.JobApplications
            .Where(ja => idsToUpdate.Contains(ja.Id))
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(ja => ja.DateTimeSyncedWithSearchUtc, pf => DateTime.UtcNow));
    }
}
using JobSearchSiteBackend.Core.Domains.PersonalFiles.RecurringJobRunners;
using JobSearchSiteBackend.Core.Domains.PersonalFiles.Search;
using JobSearchSiteBackend.Core.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Infrastructure.BackgroundJobs.Hangfire.RecurringJobsRunners;

public class SyncTextFilesWithSearchRunner(MainDataContext dbContext,
    IPersonalFileSearchRepository personalFileSearchRepository) : ISyncTextFilesWithSearchRunner
{
    public async Task Run()
    {
        var recordsToSync = await dbContext.PersonalFiles
            .Where(pf => pf.IsUploadedSuccessfully)
            .Where(pf => pf.DateTimeSyncedWithSearchUtc == null
                        || pf.DateTimeSyncedWithSearchUtc < pf.DateTimeUpdatedUtc)
            .ToListAsync();
        
        if (recordsToSync.Count == 0)
            return;

        var personalFileSearchModels = recordsToSync
            .Select(textFile => new PersonalFileSearchModel(
                textFile.Id,
                textFile.Text,
                textFile.DateTimeUpdatedUtc,
                textFile.IsDeleted
            ))
            .ToList();

        await personalFileSearchRepository.UpsertMultipleAsync(personalFileSearchModels);

        var idsToUpdate = recordsToSync.Select(x => x.Id).ToList();

        // EXECUTE UPDATE:
        // This generates a direct SQL UPDATE statement.
        // It bypasses the ChangeTracker, effectively skipping all SaveChanges interceptors.
        await dbContext.PersonalFiles
            .Where(pf => idsToUpdate.Contains(pf.Id))
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(pf => pf.DateTimeSyncedWithSearchUtc, pf => DateTime.UtcNow));
    }
}
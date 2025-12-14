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
            .Where(c => c.DateTimeSyncedWithSearchUtc == null
                        || c.DateTimeSyncedWithSearchUtc < c.DateTimeUpdatedUtc)
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

        var dateTimeSyncedUtc = DateTime.UtcNow;

        foreach (var record in recordsToSync)
        {
            record.DateTimeSyncedWithSearchUtc = dateTimeSyncedUtc;
        }
        
        await dbContext.SaveChangesAsync();
    }
}
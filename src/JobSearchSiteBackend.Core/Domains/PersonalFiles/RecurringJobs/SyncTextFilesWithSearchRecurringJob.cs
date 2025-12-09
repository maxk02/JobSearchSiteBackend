using JobSearchSiteBackend.Core.Domains.PersonalFiles.Search;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.BackgroundJobs;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.PersonalFiles.RecurringJobs;

public static class SyncTextFilesWithSearchRecurringJob
{
    public static readonly int SyncPeriodMinutes = 2;

    public static async Task Register(MainDataContext dbContext,
        IPersonalFileSearchRepository personalFileSearchRepository,
        IBackgroundJobService backgroundJobService)
    {
        backgroundJobService.AddOrUpdateRecurring("sql-search-sync-text-files",
            () => Run(dbContext, personalFileSearchRepository), $"*/{SyncPeriodMinutes} * * * *");

        await Task.CompletedTask;
    }

    public static async Task Run(MainDataContext dbContext, IPersonalFileSearchRepository personalFileSearchRepository)
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
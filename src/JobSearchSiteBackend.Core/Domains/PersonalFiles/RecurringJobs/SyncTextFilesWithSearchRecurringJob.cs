using JobSearchSiteBackend.Core.Domains.Jobs;
using JobSearchSiteBackend.Core.Domains.Jobs.Search;
using JobSearchSiteBackend.Core.Domains.PersonalFiles.Search;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.BackgroundJobs;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.PersonalFiles.RecurringJobs;

public static class SyncTextFilesWithSearchRecurringJob
{
    public static readonly int SyncPeriodMinutes = 2;
    
    public static async Task Register(MainDataContext dbContext,
        ICvOrCertificateFileSearchRepository cvOrCertificateFileSearchRepository,
        IBackgroundJobService backgroundJobService)
    {
        backgroundJobService.AddOrUpdateRecurring("sql-search-sync-text-files", 
            () => Run(dbContext, cvOrCertificateFileSearchRepository), $"*/{SyncPeriodMinutes} * * * *");
        
        await Task.CompletedTask;
    }
    
    public static async Task Run(MainDataContext dbContext, ICvOrCertificateFileSearchRepository cvOrCertificateFileSearchRepository)
    {
        var lastUpdateInfo = await dbContext
                .SqlToSearchSyncInfos
                .Where(x => x.CollectionName == "text_files")
                .SingleOrDefaultAsync();

        if (lastUpdateInfo == null)
        {
            throw new InvalidDataException();
        }

        // var vs this type??
        IQueryable<CvOrCertificateFile> query = dbContext.CvOrCertificateFiles
            .AsNoTracking()
            .Where(file => file.IsUploadedSuccessfully);
        
        if (lastUpdateInfo.UpdatedUpToDateTimeUtc != null)
        {
            query = query
                .Where(file => file.DateTimeUpdatedUtc > lastUpdateInfo.UpdatedUpToDateTimeUtc)
                .OrderBy(x => x.DateTimeUpdatedUtc);
        }

        var recordsToUpdate = await query.ToListAsync();
        
        var textFileSearchModels = recordsToUpdate.Select(textFile => new CvOrCertificateFileSearchModel(
            textFile.Id,
            textFile.Text,
            textFile.DateTimeUpdatedUtc,
            textFile.IsDeleted
        )).ToList();

        await cvOrCertificateFileSearchRepository.UpsertMultipleAsync(textFileSearchModels);
        
        lastUpdateInfo.UpdatedUpToDateTimeUtc = recordsToUpdate.Last().DateTimeUpdatedUtc;
        lastUpdateInfo.LastTimeSyncedUtc = DateTime.UtcNow;
        await dbContext.SaveChangesAsync();
    }
}
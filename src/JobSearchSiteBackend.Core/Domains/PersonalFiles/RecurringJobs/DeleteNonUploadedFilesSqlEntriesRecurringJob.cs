using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.BackgroundJobs;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.PersonalFiles.RecurringJobs;

public static class DeleteNonUploadedFilesSqlEntriesRecurringJob
{
    public static readonly int DeleteOlderThanDays = 2;
    
    public static async Task Register(MainDataContext dbContext,
        IBackgroundJobService backgroundJobService)
    {
        backgroundJobService.AddOrUpdateRecurring("sql-delete-non-uploaded-cv-or-cert-files", 
            () => Run(dbContext), $"*/{DeleteOlderThanDays * 24 * 60} * * * *");
        
        await Task.CompletedTask;
    }
    
    public static async Task Run(MainDataContext dbContext)
    {
        var query = dbContext.CvOrCertificateFiles
            .AsNoTracking()
            .Where(cv => cv.IsUploadedSuccessfully == false && cv.DateTimeUpdatedUtc < DateTime.UtcNow.AddDays(-DeleteOlderThanDays));

        var recordsToDelete = await query.ToListAsync();
        
        dbContext.CvOrCertificateFiles.RemoveRange(recordsToDelete);
        
        await dbContext.SaveChangesAsync();
    }
}
using JobSearchSiteBackend.Main.Domains.PersonalFiles.RecurringJobRunners;
using JobSearchSiteBackend.Main.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Main.Domains.PersonalFiles.RecurringJobRunners.Hangfire;

public class DeleteNonUploadedFilesRunner(MainDataContext dbContext) : IDeleteNonUploadedFilesRunner
{
    public async Task Run(int deleteOlderThanDays)
    {
        var query = dbContext.PersonalFiles
            .Where(cv => cv.IsUploadedSuccessfully == false 
                         && cv.DateTimeCreatedUtc < DateTime.UtcNow.AddDays(-deleteOlderThanDays));

        var recordsToDelete = await query.ToListAsync();
        
        if (recordsToDelete.Count == 0)
            return;
        
        dbContext.PersonalFiles.RemoveRange(recordsToDelete);
        
        await dbContext.SaveChangesAsync();
    }
}
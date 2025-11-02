using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.BackgroundJobs;
using JobSearchSiteBackend.Core.Services.FileStorage;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Companies.RecurringJobs;

public static class ClearCompanyAvatarsRecurringJob
{
    public static async Task Register(MainDataContext dbContext,
        IFileStorageService fileStorageService,
        IBackgroundJobService backgroundJobService)
    {
        backgroundJobService.AddOrUpdateRecurring("clear-company-avatars",
            () => Run(dbContext, fileStorageService), "0 0 * * *");

        await Task.CompletedTask;
    }

    public static async Task Run(MainDataContext dbContext, IFileStorageService fileStorageService)
    {
        var avatarsToClear = await dbContext.CompanyAvatars
            .Where(ca => ca.DateTimeUpdatedUtc < DateTime.UtcNow.AddMonths(-1))
            .Where(ca => ca.IsDeleted || !ca.IsUploadedSuccessfully)
            .ToListAsync(CancellationToken.None);

        List<(Guid, string)> avatarsToClearGuidExtensionTuple = avatarsToClear
            .Select(a => (a.GuidIdentifier, a.Extension))
            .ToList();

        await fileStorageService.BulkDeleteFilesAsync(avatarsToClearGuidExtensionTuple);
        
        dbContext.CompanyAvatars.RemoveRange(avatarsToClear);
        
        await dbContext.SaveChangesAsync();
    }
}
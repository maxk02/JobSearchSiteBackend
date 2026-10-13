using JobSearchSiteBackend.Core.Domains.Companies.RecurringJobRunners;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.FileStorage;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Infrastructure.BackgroundJobs.Hangfire.RecurringJobsRunners;

public class ClearCompanyAvatarsRunner(MainDataContext dbContext,
    IFileStorageService fileStorageService) : IClearCompanyAvatarsRunner
{
    public async Task Run()
    {
        var avatarsToClear = await dbContext.CompanyAvatars
            .Where(ca => ca.DateTimeUpdatedUtc < DateTime.UtcNow.AddMonths(-1))
            .Where(ca => ca.IsDeleted || !ca.IsUploadedSuccessfully)
            .ToListAsync(CancellationToken.None);

        if (avatarsToClear.Count == 0)
            return;

        List<(Guid, string)> avatarsToClearGuidExtensionTuple = avatarsToClear
            .Select(a => (a.GuidIdentifier, a.Extension))
            .ToList();

        await fileStorageService.BulkDeleteFilesAsync(FileStorageBucketName.CompanyAvatars,
            avatarsToClearGuidExtensionTuple);
        
        dbContext.CompanyAvatars.RemoveRange(avatarsToClear);
        
        await dbContext.SaveChangesAsync();
    }
}
using JobSearchSiteBackend.Main.Domains.Companies.RecurringJobRunners;
using JobSearchSiteBackend.Main.Persistence;
using JobSearchSiteBackend.Main.Services.FileStorage;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Main.Domains.Companies.RecurringJobRunners.Hangfire;

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
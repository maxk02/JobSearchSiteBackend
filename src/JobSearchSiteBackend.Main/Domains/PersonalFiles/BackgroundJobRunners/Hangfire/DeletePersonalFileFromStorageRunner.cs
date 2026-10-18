using Hangfire;
using JobSearchSiteBackend.Main.Services.FileStorage;

namespace JobSearchSiteBackend.Main.Domains.PersonalFiles.BackgroundJobRunners.Hangfire;

public class DeletePersonalFileFromStorageRunner(IBackgroundJobClient backgroundJobClient)
    : IDeletePersonalFileFromStorageRunner
{
    public async Task RunAsync(FileStorageBucketName bucketName, Guid guidIdentifier, string extension)
    {
        backgroundJobClient.Enqueue<IFileStorageService>(fileStorageService =>
            fileStorageService.DeleteFileAsync(bucketName, guidIdentifier, extension, CancellationToken.None));
        
        await Task.CompletedTask;
    }
}
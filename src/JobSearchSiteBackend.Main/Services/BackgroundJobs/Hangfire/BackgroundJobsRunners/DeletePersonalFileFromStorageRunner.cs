using Hangfire;
using JobSearchSiteBackend.Core.Domains.PersonalFiles.BackgroundJobRunners;
using JobSearchSiteBackend.Core.Services.FileStorage;

namespace JobSearchSiteBackend.Infrastructure.BackgroundJobs.Hangfire.BackgroundJobsRunners;

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
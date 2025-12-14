using JobSearchSiteBackend.Core.Services.FileStorage;

namespace JobSearchSiteBackend.Core.Domains.PersonalFiles.BackgroundJobRunners;

public interface IDeletePersonalFileFromStorageRunner
{
    public Task RunAsync(FileStorageBucketName bucketName, Guid guidIdentifier, string extension);
}
using JobSearchSiteBackend.Main.Services.FileStorage;

namespace JobSearchSiteBackend.Main.Domains.PersonalFiles.BackgroundJobRunners;

public interface IDeletePersonalFileFromStorageRunner
{
    public Task RunAsync(FileStorageBucketName bucketName, Guid guidIdentifier, string extension);
}
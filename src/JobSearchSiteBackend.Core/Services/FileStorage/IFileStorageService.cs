namespace JobSearchSiteBackend.Core.Services.FileStorage;

public interface IFileStorageService
{
    public Task BulkDeleteFilesAsync(FileStorageBucketName bucketName, ICollection<(Guid, string)> guidExtensionTuples,
        CancellationToken cancellationToken = default);
    public Task DeleteFileAsync(FileStorageBucketName bucketName, Guid guidIdentifier, string extension,
        CancellationToken cancellationToken = default);
    public Task<Stream> GetDownloadStreamAsync(FileStorageBucketName bucketName, Guid guidIdentifier, string extension,
        CancellationToken cancellationToken = default);
    public Task<string> GetDownloadUrlAsync(FileStorageBucketName bucketName, Guid guidIdentifier, string extension,
        CancellationToken cancellationToken = default);
    public Task<string> GetPublicAssetUrlAsync(Guid guidIdentifier, string extension,
        CancellationToken cancellationToken = default);
    public Task<string> UploadFileAsync(FileStorageBucketName bucketName, Stream fileStream,
        Guid guidIdentifier, string extension,
        CancellationToken cancellationToken = default);
}
namespace JobSearchSiteBackend.Core.Services.FileStorage;

public interface IFileStorageService
{
    public Task BulkDeleteFilesAsync(ICollection<(Guid, string)> guidExtensionTuples,
        CancellationToken cancellationToken = default);
    public Task DeleteFileAsync(Guid guidIdentifier, CancellationToken cancellationToken = default);
    public Task<Stream> GetDownloadStreamAsync(Guid guidIdentifier,
        CancellationToken cancellationToken = default);
    public Task<string> GetDownloadUrlAsync(Guid guidIdentifier, CancellationToken cancellationToken = default);
    public Task<string> UploadFileAsync(Stream fileStream, Guid guidIdentifier,
        CancellationToken cancellationToken = default);
}
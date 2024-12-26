namespace Core.Services.FileStorage;

public interface IFileStorageService
{
    public Task<string> UploadFileAsync(Stream fileStream, Guid guidIdentifier, string extension,
        CancellationToken cancellationToken = default);
    public Task<string> GetDownloadUrlAsync(Guid guidIdentifier, CancellationToken cancellationToken = default);
    public Task<string> DeleteFileAsync(Guid guidIdentifier, CancellationToken cancellationToken = default);
}
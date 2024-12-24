namespace Core.Services.FileStorage;

public interface IFileStorageService
{
    public Task<string> UploadFileAsync(byte[] fileContent, string fileName, string contentType,
        CancellationToken cancellationToken = default);
    public Task<string> GetDownloadUrlAsync(string fileName, CancellationToken cancellationToken = default);
}
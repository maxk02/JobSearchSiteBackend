using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using JobSearchSiteBackend.Core.Services.FileStorage;

namespace JobSearchSiteBackend.Infrastructure.FileStorage.AmazonS3;

public class AmazonS3FileStorageService(IAmazonS3 s3Client) : IFileStorageService
{
    private readonly TimeSpan _preSignedUrlExpiry = TimeSpan.FromMinutes(30);
    
    public async Task BulkDeleteFilesAsync(FileStorageBucketName bucketName,
        ICollection<(Guid, string)> guidExtensionTuples,
        CancellationToken cancellationToken = default)
    {
        var objects = guidExtensionTuples
            .Select(tuple => new KeyVersion { Key = $"{tuple.Item1}.{tuple.Item2}" }).ToList();
        
        var deleteRequest = new DeleteObjectsRequest
        {
            BucketName = bucketName.ToString(),
            Objects = objects
        };
        
        await s3Client.DeleteObjectsAsync(deleteRequest, cancellationToken);
    }
    
    public async Task DeleteFileAsync(FileStorageBucketName bucketName,
        Guid guidIdentifier, string extension, CancellationToken cancellationToken = default)
    {
        var key = $"{guidIdentifier}.{extension}";

        var deleteRequest = new DeleteObjectRequest
        {
            BucketName = bucketName.ToString(),
            Key = key
        };

        await s3Client.DeleteObjectAsync(deleteRequest, cancellationToken);
    }

    public async Task<Stream> GetDownloadStreamAsync(FileStorageBucketName bucketName,
        Guid guidIdentifier, string extension, CancellationToken cancellationToken = default)
    {
        var key = $"{guidIdentifier}.{extension}";

        var request = new GetObjectRequest
        {
            BucketName = bucketName.ToString(),
            Key = key
        };

        var response = await s3Client.GetObjectAsync(request, cancellationToken);
        return response.ResponseStream;
    }

    public async Task<string> GetDownloadUrlAsync(FileStorageBucketName bucketName,
        Guid guidIdentifier, string extension, CancellationToken cancellationToken = default)
    {
        var key = $"{guidIdentifier}.{extension}";

        var request = new GetPreSignedUrlRequest
        {
            BucketName = bucketName.ToString(),
            Key = key,
            Expires = DateTime.UtcNow.Add(_preSignedUrlExpiry),
            Verb = HttpVerb.GET
        };

        var url = await s3Client.GetPreSignedURLAsync(request);
        return url;
    }
    
    public Task<string> GetPublicAssetUrlAsync(Guid guidIdentifier, string extension, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
    
    public async Task<string> UploadFileAsync(FileStorageBucketName bucketName,
        Stream fileStream, Guid guidIdentifier,  string extension,
        CancellationToken cancellationToken = default)
    {
        var key = $"{guidIdentifier}.{extension}";

        var uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = fileStream,
            Key = key,
            BucketName = bucketName.ToString(),
            ContentType = "application/octet-stream"
        };

        var transferUtility = new TransferUtility(s3Client);
        await transferUtility.UploadAsync(uploadRequest, cancellationToken);

        return key;
    }
}
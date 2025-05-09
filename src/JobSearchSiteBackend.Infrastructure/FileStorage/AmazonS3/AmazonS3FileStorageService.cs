using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using JobSearchSiteBackend.Core.Services.FileStorage;

namespace JobSearchSiteBackend.Infrastructure.FileStorage.AmazonS3;

public class AmazonS3FileStorageService(IAmazonS3 s3Client, string bucketName) : IFileStorageService
{
    private readonly TimeSpan _preSignedUrlExpiry = TimeSpan.FromMinutes(30);

    public async Task<string> UploadFileAsync(Stream fileStream, Guid guidIdentifier,
        CancellationToken cancellationToken = default)
    {
        var key = guidIdentifier.ToString();

        var uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = fileStream,
            Key = key,
            BucketName = bucketName,
            ContentType = "application/octet-stream"
        };

        var transferUtility = new TransferUtility(s3Client);
        await transferUtility.UploadAsync(uploadRequest, cancellationToken);

        return key;
    }

    public async Task<Stream> GetDownloadStreamAsync(Guid guidIdentifier, CancellationToken cancellationToken = default)
    {
        var key = guidIdentifier.ToString();

        var request = new GetObjectRequest
        {
            BucketName = bucketName,
            Key = key
        };

        var response = await s3Client.GetObjectAsync(request, cancellationToken);
        return response.ResponseStream;
    }

    public async Task<string> GetDownloadUrlAsync(Guid guidIdentifier, CancellationToken cancellationToken = default)
    {
        var key = guidIdentifier.ToString();

        var request = new GetPreSignedUrlRequest
        {
            BucketName = bucketName,
            Key = key,
            Expires = DateTime.UtcNow.Add(_preSignedUrlExpiry),
            Verb = HttpVerb.GET
        };

        var url = await s3Client.GetPreSignedURLAsync(request);
        return url;
    }

    public async Task DeleteFileAsync(Guid guidIdentifier, CancellationToken cancellationToken = default)
    {
        var key = guidIdentifier.ToString();

        var deleteRequest = new DeleteObjectRequest
        {
            BucketName = bucketName,
            Key = key
        };

        await s3Client.DeleteObjectAsync(deleteRequest, cancellationToken);
    }
}
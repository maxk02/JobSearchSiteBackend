using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.PersonalFiles.Search;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Core.Services.BackgroundJobs;
using Core.Services.FileStorage;
using Core.Services.TextExtraction;
using Microsoft.Extensions.Caching.Memory;
using Shared.Result;

namespace Core.Domains.PersonalFiles.UseCases.UploadFile;

public class UploadFileHandler(
    ICurrentAccountService currentAccountService,
    IFileStorageService fileStorageService,
    IBackgroundJobService backgroundJobService,
    ITextExtractionService textExtractionService,
    IPersonalFileSearchRepository personalFileSearchRepository,
    IMemoryCache memoryCache,
    MainDataContext context) : IRequestHandler<UploadFileRequest, Result>
{
    public async Task<Result> Handle(UploadFileRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        using var memoryStream = new MemoryStream();
        await request.FileStream.CopyToAsync(memoryStream, cancellationToken);
        
        var newFile = new PersonalFile(currentUserId, request.FileName,
            request.Extension, memoryStream.Length);

        await fileStorageService.UploadFileAsync(memoryStream, newFile.GuidIdentifier, cancellationToken);

        try
        {
            context.PersonalFiles.Add(newFile);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            backgroundJobService.Enqueue(
                () => fileStorageService.DeleteFileAsync(newFile.GuidIdentifier, CancellationToken.None),
                BackgroundJobQueues.PersonalFileStorage);
            throw;
        }

        const int retryAttempts = 70;
        var retryInterval = TimeSpan.FromHours(1);
        var retainCacheFor = retryInterval * (retryAttempts + 2);
        var memCacheFileSize = (int)Math.Round((double)newFile.Size / 1024 / 1024);

        try
        {
            memoryCache.Set(newFile.GuidIdentifier, memoryStream.ToArray(),
                new MemoryCacheEntryOptions().SetSize(memCacheFileSize).SetAbsoluteExpiration(retainCacheFor));
        }
        catch
        {
            // ignored
        }

        backgroundJobService.Enqueue(
            () => ProcessFileAsync(newFile.GuidIdentifier, newFile.Id, newFile.Extension, memCacheFileSize, retainCacheFor),
            BackgroundJobQueues.PersonalFileTextExtractionAndSearch, retryAttempts, retryInterval
        );

        return Result.Success();
    }


    private async Task ProcessFileAsync(Guid fileGuid, long fileId, string fileExtension,
        long memCacheFileSize, TimeSpan retainCacheFor)
    {
        memoryCache.TryGetValue(fileGuid, out byte[]? fileBytes);

        if (fileBytes is null)
        {
            await using var downloadStream = await fileStorageService
                .GetDownloadStreamAsync(fileGuid, CancellationToken.None);

            using var memoryStream = new MemoryStream();
            await downloadStream.CopyToAsync(memoryStream);
            fileBytes = memoryStream.ToArray();

            memoryCache.Set(fileGuid, fileBytes,
                new MemoryCacheEntryOptions().SetSize(memCacheFileSize).SetAbsoluteExpiration(retainCacheFor));
        }

        memoryCache.TryGetValue($"{fileGuid.ToString()}_text", out string? text);

        if (text is null)
        {
            text = await textExtractionService.ExtractTextAsync(fileBytes, fileExtension, CancellationToken.None);

            memoryCache.Set($"{fileGuid.ToString()}_text", text, 
                new MemoryCacheEntryOptions().SetSize(1).SetAbsoluteExpiration(retainCacheFor));
        }

        if (text != "")
        {
            await personalFileSearchRepository.AddOrSetConstFieldsAsync(new PersonalFileSearchModel(fileId, text));
        }
    }
}
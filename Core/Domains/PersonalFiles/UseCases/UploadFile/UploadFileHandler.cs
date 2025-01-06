using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.PersonalFiles.Search;
using Core.Persistence.EfCore;
using Core.Services.Auth.Authentication;
using Core.Services.BackgroundJobs;
using Core.Services.FileStorage;
using Core.Services.TextExtractor;
using Shared.Result;

namespace Core.Domains.PersonalFiles.UseCases.UploadFile;

public class UploadFileHandler(
    ICurrentAccountService currentAccountService,
    IFileStorageService fileStorageService,
    IBackgroundJobService backgroundJobService,
    ITextExtractor textExtractor,
    IPersonalFileSearchRepository personalFileSearchRepository,
    MainDataContext context) : IRequestHandler<UploadFileRequest, Result>
{
    public async Task<Result> Handle(UploadFileRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var creationResult = PersonalFile.Create(currentUserId, request.FileName,
            request.Extension, request.FileStream.Length);
        if (creationResult.IsFailure)
            return Result.WithMetadataFrom(creationResult);
        var newFile = creationResult.Value;

        //starting transaction to be able to use SaveChangesAsync multiple times and revert all changes if something fails
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        context.PersonalFiles.Add(newFile);
        await context.SaveChangesAsync(cancellationToken);

        var fileId = newFile.Id;

        await fileStorageService.UploadFileAsync(request.FileStream, newFile.GuidIdentifier,
            newFile.Extension, cancellationToken);

        try
        {
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            backgroundJobService.Enqueue(
                () => fileStorageService.DeleteFileAsync(newFile.GuidIdentifier, CancellationToken.None),
                BackgroundJobQueues.PersonalFileStorage);
            throw;
        }

        return Result.Success();
    }
}
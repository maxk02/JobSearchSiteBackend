using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.BackgroundJobs;
using JobSearchSiteBackend.Core.Services.FileStorage;
using Ardalis.Result;
using JobSearchSiteBackend.Core.Persistence;

namespace JobSearchSiteBackend.Core.Domains.PersonalFiles.UseCases.DeleteFile;

public class DeleteFileHandler(
    ICurrentAccountService currentAccountService,
    IFileStorageService fileStorageService,
    IBackgroundJobService backgroundJobService,
    MainDataContext context) : IRequestHandler<DeleteFileRequest, Result>
{
    public async Task<Result> Handle(DeleteFileRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();
        
        var personalFile = await context.PersonalFiles.FindAsync([request.Id], cancellationToken);

        if (personalFile is null)
            return Result.NotFound();

        if (personalFile.UserId != currentUserId)
            return Result.Forbidden();
        
        context.PersonalFiles.Remove(personalFile);
        await context.SaveChangesAsync(cancellationToken);

        backgroundJobService.Enqueue(
            () => fileStorageService.DeleteFileAsync(personalFile.GuidIdentifier, CancellationToken.None),
            BackgroundJobQueues.PersonalFileStorage);

        return Result.Success();
    }
}
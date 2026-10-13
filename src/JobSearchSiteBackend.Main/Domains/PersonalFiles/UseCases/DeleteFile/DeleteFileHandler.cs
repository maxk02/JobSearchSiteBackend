using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.FileStorage;
using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains.PersonalFiles.BackgroundJobRunners;
using JobSearchSiteBackend.Core.Persistence;

namespace JobSearchSiteBackend.Core.Domains.PersonalFiles.UseCases.DeleteFile;

public class DeleteFileHandler(
    ICurrentAccountService currentAccountService,
    IDeletePersonalFileFromStorageRunner deletePersonalFileFromStorageRunner,
    MainDataContext context) : IRequestHandler<DeleteFileCommand, Result>
{
    public async Task<Result> Handle(DeleteFileCommand command, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();
        
        var personalFile = await context.PersonalFiles.FindAsync([command.Id], cancellationToken);

        if (personalFile is null)
            return Result.NotFound();

        if (personalFile.UserId != currentUserId)
            return Result.Forbidden();
        
        context.PersonalFiles.Remove(personalFile);
        await context.SaveChangesAsync(cancellationToken);

        await deletePersonalFileFromStorageRunner.RunAsync(FileStorageBucketName.PersonalFiles,
            personalFile.GuidIdentifier, personalFile.Extension);

        return Result.Success();
    }
}
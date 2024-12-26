using Core.Domains._Shared.Repositories;
using Core.Domains._Shared.UseCaseStructure;
using Core.Services.Auth.Authentication;
using Core.Services.FileStorage;
using Core.Services.FileStorage.JobSchedulers;
using Shared.Result;

namespace Core.Domains.PersonalFiles.UseCases.DeleteFile;

public class DeleteFileHandler(
    ICurrentAccountService currentAccountService,
    IFileStorageService fileStorageService,
    IDeleteFileFromStorageScheduler deleteFileFromStorageScheduler,
    IRepository<PersonalFile> personalFileRepository) : IRequestHandler<DeleteFileRequest, Result>
{
    public async Task<Result> Handle(DeleteFileRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();
        
        var personalFile = await personalFileRepository.GetByIdAsync(request.FileId, cancellationToken);

        if (personalFile is null)
            return Result.NotFound();
        
        if (personalFile.UserId != currentUserId)
            return Result.Forbidden();
        
        await personalFileRepository.RemoveByIdAsync(request.FileId, cancellationToken);
        
        try
        {
            await fileStorageService.DeleteFileAsync(personalFile.GuidIdentifier, cancellationToken);
        }
        catch
        {
            await deleteFileFromStorageScheduler.ScheduleAsync(personalFile.GuidIdentifier);
        }
        
        return Result.Success();
    }
}
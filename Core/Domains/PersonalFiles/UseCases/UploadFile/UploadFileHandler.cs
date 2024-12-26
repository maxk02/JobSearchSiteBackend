using Core.Domains._Shared.Repositories;
using Core.Domains._Shared.UnitOfWork;
using Core.Domains._Shared.UseCaseStructure;
using Core.Services.Auth.Authentication;
using Core.Services.FileStorage;
using Core.Services.FileStorage.JobSchedulers;
using Shared.Result;

namespace Core.Domains.PersonalFiles.UseCases.UploadFile;

public class UploadFileHandler(
    ICurrentAccountService currentAccountService,
    IFileStorageService fileStorageService,
    IDeleteFileFromStorageScheduler deleteFileFromStorageScheduler,
    IUnitOfWork unitOfWork,
    IRepository<PersonalFile> personalFileRepository) : IRequestHandler<UploadFileRequest, Result>
{
    public async Task<Result> Handle(UploadFileRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var creationResult = PersonalFile.Create(currentUserId, request.FileName,
            request.Extension, request.FileStream.Length);
        if (creationResult.IsFailure)
            return Result.WithMetadataFrom(creationResult);
        var newFile = creationResult.Value;
        
        await unitOfWork.BeginAsync(cancellationToken);
        await personalFileRepository.AddAsync(newFile, cancellationToken);
        
        await fileStorageService.UploadFileAsync(request.FileStream, newFile.GuidIdentifier,
            newFile.Extension, cancellationToken);

        try
        {
            await unitOfWork.CommitAsync(cancellationToken);
        }
        catch
        {
            try
            {
                await fileStorageService.DeleteFileAsync(newFile.GuidIdentifier, cancellationToken);
            }
            catch
            {
                await deleteFileFromStorageScheduler.ScheduleAsync(newFile.GuidIdentifier);
                throw;
            }
            throw;
        }

        return Result.Success();
    }
}
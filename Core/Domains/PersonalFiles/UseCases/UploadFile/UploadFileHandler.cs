using Core.Domains._Shared.UseCaseStructure;
using Core.Services.Auth.Authentication;
using Core.Services.FileStorage;
using Shared.Result;

namespace Core.Domains.PersonalFiles.UseCases.UploadFile;

public class UploadFileHandler(ICurrentAccountService currentAccountService,
    IFileStorageService fileStorageService,
    IPersonalFileRepository personalFileRepository) : IRequestHandler<UploadFileRequest, Result>
{
    public async Task<Result> Handle(UploadFileRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();
        
        var creationResult = PersonalFile.Create(currentUserId, request.FileName,
            request.ContentType, request.FileContent.Length);
        if (creationResult.IsFailure)
            return Result.WithMetadataFrom(creationResult);
        
        await fileStorageService.UploadFileAsync(request.FileContent, request.FileName, request.ContentType, cancellationToken);

        await personalFileRepository.AddAsync(creationResult.Value, cancellationToken);
        
        return Result.Success();
    }
}
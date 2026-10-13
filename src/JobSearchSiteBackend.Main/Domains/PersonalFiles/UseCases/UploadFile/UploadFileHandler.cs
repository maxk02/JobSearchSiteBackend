using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.FileStorage;
using JobSearchSiteBackend.Core.Services.TextExtraction;

namespace JobSearchSiteBackend.Core.Domains.PersonalFiles.UseCases.UploadFile;

public class UploadFileHandler(
    ICurrentAccountService currentAccountService,
    IFileStorageService fileStorageService,
    ITextExtractionService textExtractionService,
    MainDataContext context) : IRequestHandler<UploadFileCommand, Result<UploadFileResult>>
{
    public async Task<Result<UploadFileResult>> Handle(UploadFileCommand command, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();
        
        var text = await textExtractionService.ExtractTextAsync(command.FileStream, command.Extension, cancellationToken);
        
        var newFile = new PersonalFile(currentUserId, command.Name, command.Extension, command.Size, text);
        
        context.PersonalFiles.Add(newFile);
        await context.SaveChangesAsync(cancellationToken);

        await fileStorageService.UploadFileAsync(FileStorageBucketName.PersonalFiles,
            command.FileStream, newFile.GuidIdentifier, newFile.Extension, cancellationToken);
        
        newFile.IsUploadedSuccessfully = true;
        context.PersonalFiles.Update(newFile);
        await context.SaveChangesAsync(cancellationToken);

        var response = new UploadFileResult(newFile.Id);
        
        return Result.Success(response);
    }
}
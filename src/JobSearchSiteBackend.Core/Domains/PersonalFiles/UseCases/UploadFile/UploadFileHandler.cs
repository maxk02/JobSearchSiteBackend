using System.Transactions;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.PersonalFiles.Search;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.BackgroundJobs;
using JobSearchSiteBackend.Core.Services.FileStorage;
using JobSearchSiteBackend.Core.Services.TextExtraction;
using Microsoft.Extensions.Caching.Memory;
using Ardalis.Result;
using JobSearchSiteBackend.Core.Persistence;

namespace JobSearchSiteBackend.Core.Domains.PersonalFiles.UseCases.UploadFile;

public class UploadFileHandler(
    ICurrentAccountService currentAccountService,
    IFileStorageService fileStorageService,
    ITextExtractionService textExtractionService,
    MainDataContext context) : IRequestHandler<UploadFileRequest, Result<UploadFileResponse>>
{
    public async Task<Result<UploadFileResponse>> Handle(UploadFileRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();
        
        var text = await textExtractionService.ExtractTextAsync(request.FileStream, request.Extension, cancellationToken);
        
        var newFile = new PersonalFile(currentUserId, request.Name, request.Extension, request.Size, text);
        
        context.PersonalFiles.Add(newFile);
        await context.SaveChangesAsync(cancellationToken);

        await fileStorageService.UploadFileAsync(FileStorageBucketName.PersonalFiles,
            request.FileStream, newFile.GuidIdentifier, newFile.Extension, cancellationToken);
        
        newFile.IsUploadedSuccessfully = true;
        context.PersonalFiles.Update(newFile);
        await context.SaveChangesAsync(cancellationToken);

        var response = new UploadFileResponse(newFile.Id);
        
        return Result.Success(response);
    }
}
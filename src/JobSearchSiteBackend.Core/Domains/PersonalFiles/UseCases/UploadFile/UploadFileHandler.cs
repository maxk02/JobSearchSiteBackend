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
    MainDataContext context) : IRequestHandler<UploadFileRequest, Result>
{
    public async Task<Result> Handle(UploadFileRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();
        
        if (request.FormFile is null || request.FormFile.Length == 0)
        {
            return Result.Invalid();
        }

        using var memoryStream = new MemoryStream();
        await request.FormFile.CopyToAsync(memoryStream, cancellationToken);
        
        var fileName = Path.GetFileNameWithoutExtension(request.FormFile.FileName);
        var extension = Path.GetExtension(request.FormFile.FileName).TrimStart('.');
        
        var fileBytes = memoryStream.ToArray();
        var text = await textExtractionService.ExtractTextAsync(fileBytes, extension, CancellationToken.None);
        
        var newFile = new PersonalFile(currentUserId, fileName, extension, memoryStream.Length, text);
        
        context.PersonalFiles.Add(newFile);
        await context.SaveChangesAsync(cancellationToken);

        await fileStorageService.UploadFileAsync(memoryStream, newFile.GuidIdentifier, cancellationToken);
        
        newFile.IsUploadedSuccessfully = true;
        context.PersonalFiles.Update(newFile);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
using Ardalis.Result;
using AutoMapper;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.FileStorage;

namespace JobSearchSiteBackend.Core.Domains.PersonalFiles.UseCases.GetDownloadLink;

public class GetDownloadLinkHandler(
    MainDataContext context,
    IMapper mapper,
    ICurrentAccountService currentAccountService,
    IFileStorageService fileStorageService)
    : IRequestHandler<GetDownloadLinkRequest, Result<GetDownloadLinkResponse>>
{
    public async Task<Result<GetDownloadLinkResponse>> Handle(GetDownloadLinkRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();
        
        var fileSqlRecord = await context.PersonalFiles.FindAsync([request.FileId], cancellationToken);
        
        if (fileSqlRecord is null)
            return Result.NotFound();
        
        if (fileSqlRecord.UserId != currentUserId)
            return Result.Forbidden();
        
        if (!fileSqlRecord.IsUploadedSuccessfully)
            return Result.Error();

        var link = await fileStorageService.GetDownloadUrlAsync(FileStorageBucketName.PersonalFiles,
            fileSqlRecord.GuidIdentifier, fileSqlRecord.Extension, cancellationToken);

        var response = new GetDownloadLinkResponse(link);
        
        return Result.Success(response);
    }
}
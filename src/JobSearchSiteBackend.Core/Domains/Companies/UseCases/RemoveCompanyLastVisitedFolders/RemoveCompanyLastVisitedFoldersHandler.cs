using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.Caching;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.RemoveCompanyLastVisitedFolders;

public class RemoveCompanyLastVisitedFoldersHandler(
    ICurrentAccountService currentAccountService,
    ICompanyLastVisitedFoldersCacheRepository cacheRepo
    ) : IRequestHandler<RemoveCompanyLastVisitedFoldersRequest, Result>
{
    public async Task<Result> Handle(RemoveCompanyLastVisitedFoldersRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        if (request.SingleFolderId.HasValue)
        {
            await cacheRepo.DeleteOneLastVisitedAsync(currentUserId.ToString(),
                request.CompanyId.ToString(), request.SingleFolderId.Value.ToString());
        }
        else
        {
            await cacheRepo.DeleteAllLastVisitedAsync(currentUserId.ToString(), request.CompanyId.ToString());
        }

        return Result.Success();
    }
}
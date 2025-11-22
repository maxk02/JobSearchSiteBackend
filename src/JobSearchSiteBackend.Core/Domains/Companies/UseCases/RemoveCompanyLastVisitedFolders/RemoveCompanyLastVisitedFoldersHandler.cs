using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.Caching;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.RemoveCompanyLastVisitedFolders;

public class RemoveCompanyLastVisitedFoldersHandler(
    ICurrentAccountService currentAccountService,
    ICompanyLastVisitedFoldersCacheRepository cacheRepo
    ) : IRequestHandler<RemoveCompanyLastVisitedFoldersCommand, Result>
{
    public async Task<Result> Handle(RemoveCompanyLastVisitedFoldersCommand command,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        if (command.SingleFolderId.HasValue)
        {
            await cacheRepo.DeleteOneLastVisitedAsync(currentUserId.ToString(),
                command.CompanyId.ToString(), command.SingleFolderId.Value.ToString());
        }
        else
        {
            await cacheRepo.DeleteAllLastVisitedAsync(currentUserId.ToString(), command.CompanyId.ToString());
        }

        return Result.Success();
    }
}
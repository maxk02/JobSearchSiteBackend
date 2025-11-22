using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.Caching;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.RemoveCompanyLastVisitedJobs;

public class RemoveCompanyLastVisitedJobsHandler(
    ICurrentAccountService currentAccountService,
    ICompanyLastVisitedJobsCacheRepository cacheRepo
    ) : IRequestHandler<RemoveCompanyLastVisitedJobsCommand, Result>
{
    public async Task<Result> Handle(RemoveCompanyLastVisitedJobsCommand command,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        if (command.SingleJobId.HasValue)
        {
            await cacheRepo.DeleteOneLastVisitedAsync(currentUserId.ToString(),
                command.CompanyId.ToString(), command.SingleJobId.Value.ToString());
        }
        else
        {
            await cacheRepo.DeleteAllLastVisitedAsync(currentUserId.ToString(), command.CompanyId.ToString());
        }

        return Result.Success();
    }
}
using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.Caching;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.RemoveCompanyLastVisitedJobs;

public class RemoveCompanyLastVisitedJobsHandler(
    ICurrentAccountService currentAccountService,
    ICompanyLastVisitedJobsCacheRepository cacheRepo
    ) : IRequestHandler<RemoveCompanyLastVisitedJobsRequest, Result>
{
    public async Task<Result> Handle(RemoveCompanyLastVisitedJobsRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        if (request.SingleJobId.HasValue)
        {
            await cacheRepo.DeleteOneLastVisitedAsync(currentUserId.ToString(),
                request.CompanyId.ToString(), request.SingleJobId.Value.ToString());
        }
        else
        {
            await cacheRepo.DeleteAllLastVisitedAsync(currentUserId.ToString(), request.CompanyId.ToString());
        }

        return Result.Success();
    }
}
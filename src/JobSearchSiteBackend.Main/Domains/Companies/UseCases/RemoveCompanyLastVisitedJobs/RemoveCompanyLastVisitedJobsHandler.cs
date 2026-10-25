using Ardalis.Result;
using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Main.Services.Auth;

namespace JobSearchSiteBackend.Main.Domains.Companies.UseCases.RemoveCompanyLastVisitedJobs;

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
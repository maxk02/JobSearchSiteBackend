using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Companies.Dtos;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.Caching;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyLastVisitedJobs;

public class GetCompanyLastVisitedJobsHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context,
    ICompanyLastVisitedJobsCacheRepository cacheRepo)
    : IRequestHandler<GetCompanyLastVisitedJobsQuery, Result<GetCompanyLastVisitedJobsResult>>
{
    public async Task<Result<GetCompanyLastVisitedJobsResult>> Handle(GetCompanyLastVisitedJobsQuery query,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var idListFromCache = await cacheRepo
            .GetLastVisitedAsync(currentUserId.ToString(), query.CompanyId.ToString());

        if (idListFromCache.Count == 0)
        {
            var emptyResult = new GetCompanyLastVisitedJobsResult([]);
            return Result.Success(emptyResult);
        }
        
        var jobListItemDtos = await context.Jobs
            .Where(job => job.CompanyId == query.CompanyId)
            .Where(jobListItem => idListFromCache.Contains(jobListItem.Id))
            .Select(job => new CompanyJobListItemDto(job.Id, job.Title))
            .ToListAsync(cancellationToken);

        var result = new GetCompanyLastVisitedJobsResult(jobListItemDtos);

        return Result.Success(result);
    }
}
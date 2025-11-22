using Ardalis.Result;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Companies.Dtos;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyLastVisitedFolders;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.Caching;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyLastVisitedJobs;

public class GetCompanyLastVisitedJobsHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context,
    IMapper mapper,
    ICompanyLastVisitedJobsCacheRepository cacheRepo
    ): IRequestHandler<GetCompanyLastVisitedJobsQuery,
    Result<GetCompanyLastVisitedJobsResult>>
{
    public async Task<Result<GetCompanyLastVisitedJobsResult>> Handle(GetCompanyLastVisitedJobsQuery query,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var idListFromCache = await cacheRepo
            .GetLastVisitedAsync(currentUserId.ToString(), query.CompanyId.ToString());
        
        var jobListItemDtos = await context.JobFolders
            .Where(jf => jf.CompanyId == query.CompanyId)
            .Where(jf =>
                jf.UserJobFolderClaims!.Any(jfc =>
                    jfc.ClaimId == JobFolderClaim.CanReadJobs.Id && jfc.UserId == currentUserId))
            .SelectMany(jf => jf.Jobs!)
            .Where(jf => idListFromCache.Contains(jf.Id))
            .ProjectTo<CompanyJobListItemDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        var response = new GetCompanyLastVisitedJobsResult(jobListItemDtos);

        return Result.Success(response);
    }
}
using Ardalis.Result;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims;
using JobSearchSiteBackend.Core.Domains.Jobs.Dtos;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.Caching;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.GetJobs;

public class GetJobsHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context,
    IMapper mapper,
    ICompanyLastVisitedFoldersCacheRepository cacheRepo) : IRequestHandler<GetJobsQuery, Result<GetJobsResult>>
{
    public async Task<Result<GetJobsResult>> Handle(GetJobsQuery query,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var jobFolder = await context.JobFolders.FindAsync([query.Id], cancellationToken);
        if (jobFolder is null)
            return Result<GetJobsResult>.NotFound();

        var hasReadClaim = await context.JobFolderRelations
            .GetThisOrAncestorWhereUserHasClaim(query.Id, currentUserId,
                JobFolderClaim.CanReadJobs.Id)
            .AnyAsync(cancellationToken);

        if (!hasReadClaim)
            return Result<GetJobsResult>.Forbidden();

        await cacheRepo.AddLastVisitedAsync(currentUserId.ToString(),
            jobFolder.CompanyId.ToString(), jobFolder.Id.ToString());
        
        var childJobInfoDtos = await context.Jobs
            .Where(job => job.JobFolderId == query.Id)
            .ProjectTo<JobCardDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new GetJobsResult(childJobInfoDtos);
    }
}
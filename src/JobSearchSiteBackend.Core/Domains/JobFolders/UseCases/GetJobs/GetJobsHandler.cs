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
    ICompanyLastVisitedFoldersCacheRepository cacheRepo) : IRequestHandler<GetJobsRequest, Result<GetJobsResponse>>
{
    public async Task<Result<GetJobsResponse>> Handle(GetJobsRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var jobFolder = await context.JobFolders.FindAsync([request.Id], cancellationToken);
        if (jobFolder is null)
            return Result<GetJobsResponse>.NotFound();

        var hasReadClaim = await context.JobFolderRelations
            .GetThisOrAncestorWhereUserHasClaim(request.Id, currentUserId,
                JobFolderClaim.CanReadJobs.Id)
            .AnyAsync(cancellationToken);

        if (!hasReadClaim)
            return Result<GetJobsResponse>.Forbidden();

        await cacheRepo.AddLastVisitedAsync(currentUserId.ToString(),
            jobFolder.CompanyId.ToString(), jobFolder.Id.ToString());
        
        var childJobInfoDtos = await context.Jobs
            .Where(job => job.JobFolderId == request.Id)
            .ProjectTo<JobCardDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new GetJobsResponse(childJobInfoDtos);
    }
}
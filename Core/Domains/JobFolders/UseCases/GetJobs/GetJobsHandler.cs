using Ardalis.Result;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.JobFolderClaims;
using Core.Domains.Jobs.Dtos;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace Core.Domains.JobFolders.UseCases.GetJobs;

public class GetJobsHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context,
    IMapper mapper) : IRequestHandler<GetJobsRequest, Result<GetJobsResponse>>
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
                JobFolderClaim.CanReadJobsAndSubfolders.Id)
            .AnyAsync(cancellationToken);

        if (!hasReadClaim)
            return Result<GetJobsResponse>.Forbidden();
        
        var childJobInfoDtos = await context.Jobs
            .Where(job => job.JobFolderId == request.Id)
            .ProjectTo<JobCardDto>(mapper.ConfigurationProvider)
            // .Select(job => mapper.Map<JobCardDto>(job))
            .ToListAsync(cancellationToken);

        return new GetJobsResponse(childJobInfoDtos);
    }
}
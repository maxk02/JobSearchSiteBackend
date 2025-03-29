using Ardalis.Result;
using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.JobFolderClaims;
using Core.Domains.Jobs.Dtos;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace Core.Domains.JobFolders.UseCases.GetJobs;

public class GetJobsHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<GetJobsRequest, Result<GetJobsResponse>>
{
    public async Task<Result<GetJobsResponse>> Handle(GetJobsRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var jobFolder = await context.JobFolders.FindAsync([request.JobFolderId], cancellationToken);
        if (jobFolder is null)
            return Result<GetJobsResponse>.NotFound();

        var hasReadClaim = await context.JobFolderRelations
            .GetThisOrAncestorWhereUserHasClaim(request.JobFolderId, currentUserId,
                JobFolderClaim.CanReadJobsAndSubfolders.Id)
            .AnyAsync(cancellationToken);

        if (!hasReadClaim)
            return Result<GetJobsResponse>.Forbidden();

        throw new NotImplementedException();
        
        var childJobInfoDtos = await context.Jobs
            .Where(job => job.JobFolderId == request.JobFolderId)
            .Select(job => new JobInfoDto(job.Id, 1, job.CategoryId, job.Title, job.DateTimePublishedUtc,
                job.DateTimeExpiringUtc, job.SalaryRecord, job.EmploymentTypeRecord))
            .ToListAsync(cancellationToken);

        return new GetJobsResponse(childJobInfoDtos);
    }
}
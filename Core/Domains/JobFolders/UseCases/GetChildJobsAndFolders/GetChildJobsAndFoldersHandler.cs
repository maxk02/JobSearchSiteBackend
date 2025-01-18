using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.JobFolderClaims;
using Core.Domains.JobFolders.Dtos;
using Core.Domains.Jobs.Dtos;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;

namespace Core.Domains.JobFolders.UseCases.GetChildJobsAndFolders;

public class GetChildJobsAndFoldersHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<GetChildJobsAndFoldersRequest, Result<GetChildJobsAndFoldersResponse>>
{
    public async Task<Result<GetChildJobsAndFoldersResponse>> Handle(GetChildJobsAndFoldersRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var jobFolder = await context.JobFolders.FindAsync([request.JobFolderId], cancellationToken);
        if (jobFolder is null)
            return Result<GetChildJobsAndFoldersResponse>.NotFound();

        var hasReadClaim = await context.JobFolderRelations
            .GetThisOrAncestorWhereUserHasClaim(request.JobFolderId, currentUserId,
                JobFolderClaim.CanReadJobsAndSubfolders.Id)
            .AnyAsync(cancellationToken);

        if (!hasReadClaim)
            return Result<GetChildJobsAndFoldersResponse>.Forbidden();
        
        var childJobInfocardDtos = await context.Jobs
            .Where(job => job.JobFolderId == request.JobFolderId)
            .Select(job => new JobInfocardInFolderDto(job.Id, job.CategoryId, job.Title, job.DateTimePublishedUtc,
                job.DateTimeExpiringUtc, job.SalaryRecord, job.EmploymentTypeRecord))
            .ToListAsync(cancellationToken);

        var childJobFolderDtos = await context.JobFolderRelations
            .Where(jfc => jfc.AncestorId == request.JobFolderId)
            .Where(jfc => jfc.Depth == 1)
            .Select(jfc => new JobFolderDto(jfc.Descendant!.Id, jfc.Descendant!.Name!, jfc.Descendant.Description))
            .ToListAsync(cancellationToken);

        return new GetChildJobsAndFoldersResponse(childJobInfocardDtos, childJobFolderDtos);
    }
}
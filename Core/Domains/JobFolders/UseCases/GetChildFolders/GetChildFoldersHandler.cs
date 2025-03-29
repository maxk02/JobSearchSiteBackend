using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.JobFolderClaims;
using Core.Domains.JobFolders.Dtos;
using Core.Domains.Jobs.Dtos;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;

namespace Core.Domains.JobFolders.UseCases.GetChildFolders;

public class GetChildFoldersHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<GetChildFoldersRequest, Result<GetChildFoldersResponse>>
{
    public async Task<Result<GetChildFoldersResponse>> Handle(GetChildFoldersRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var jobFolder = await context.JobFolders.FindAsync([request.JobFolderId], cancellationToken);
        if (jobFolder is null)
            return Result<GetChildFoldersResponse>.NotFound();

        var hasReadClaim = await context.JobFolderRelations
            .GetThisOrAncestorWhereUserHasClaim(request.JobFolderId, currentUserId,
                JobFolderClaim.CanReadJobsAndSubfolders.Id)
            .AnyAsync(cancellationToken);

        if (!hasReadClaim)
            return Result<GetChildFoldersResponse>.Forbidden();

        var childJobFolderDtos = await context.JobFolderRelations
            .Where(jfc => jfc.AncestorId == request.JobFolderId)
            .Where(jfc => jfc.Depth == 1)
            .Select(jfc => new JobFolderDto(jfc.Descendant!.Id, jfc.Descendant!.Name!, jfc.Descendant.Description))
            .ToListAsync(cancellationToken);

        return new GetChildFoldersResponse(childJobFolderDtos);
    }
}
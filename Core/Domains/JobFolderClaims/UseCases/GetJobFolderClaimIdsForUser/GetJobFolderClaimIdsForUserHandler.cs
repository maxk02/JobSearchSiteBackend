using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.CompanyClaims;
using Core.Domains.JobFolders;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Shared.Result;

namespace Core.Domains.JobFolderClaims.UseCases.GetJobFolderClaimIdsForUser;

public class GetJobFolderClaimIdsForUserHandler(ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<GetJobFolderClaimIdsForUserRequest, Result<ICollection<long>>>
{
    public async Task<Result<ICollection<long>>> Handle(GetJobFolderClaimIdsForUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();
        
        var currentUserClaimIdsOnThisAndAncestors = await context.JobFolderRelations
            .GetClaimIdsForThisAndAncestors(request.FolderId, currentUserId)
            .ToListAsync(cancellationToken);
        
        if (request.UserId == currentUserId)
            return currentUserClaimIdsOnThisAndAncestors;
        
        if (!currentUserClaimIdsOnThisAndAncestors.Contains(JobFolderClaim.IsAdmin.Id))
            return Result<ICollection<long>>.Forbidden("Current user is not a folder admin.");
        
        var targetUserClaimIdsOnThisAndAncestors = await context.JobFolderRelations
            .GetClaimIdsForThisAndAncestors(request.FolderId, request.UserId)
            .ToListAsync(cancellationToken);

        var visibleClaimIds =
            currentUserClaimIdsOnThisAndAncestors
                .Intersect(targetUserClaimIdsOnThisAndAncestors)
                .ToList();
        
        return visibleClaimIds;
    }
}
using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.JobFolders;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Shared.Result;
using Shared.Result.FluentValidation;

namespace Core.Domains.JobFolderClaims.UseCases.UpdateJobFolderClaimIdsForUser;

public class UpdateJobFolderClaimIdsForUserHandler(ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<UpdateJobFolderClaimIdsForUserRequest, Result>
{
    public async Task<Result> Handle(UpdateJobFolderClaimIdsForUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var permissionsValidator = new JobFolderClaimIdCollectionValidator();
        var validationResult = permissionsValidator.Validate(request.FolderPermissionIds);
        if (!validationResult.IsValid)
            return Result.Invalid(validationResult.AsErrors());
        
        var currentUserClaimIdsOnThisAndAncestors = await context.JobFolderClosures
            .GetDistinctClaimIdsForThisAndAncestors(request.FolderId, currentUserId)
            .ToListAsync(cancellationToken);
        
        if (!currentUserClaimIdsOnThisAndAncestors.Contains(JobFolderClaim.IsAdmin.Id))
            return Result.Forbidden("Current user is not a folder admin.");
        
        if (request.FolderPermissionIds.Except(currentUserClaimIdsOnThisAndAncestors).Any())
            return Result.Forbidden();
        
        var targetUserClaimIdsOnThisAndAncestors = await context.JobFolderClosures
            .GetDistinctClaimIdsForThisAndAncestors(request.FolderId, request.UserId)
            .ToListAsync(cancellationToken);
        
        if (targetUserClaimIdsOnThisAndAncestors.Contains(JobFolderClaim.IsOwner.Id))
            return Result.Forbidden();
        
        if (targetUserClaimIdsOnThisAndAncestors.Contains(JobFolderClaim.IsAdmin.Id) &&
            !currentUserClaimIdsOnThisAndAncestors.Contains(JobFolderClaim.IsOwner.Id))
            return Result.Forbidden();
        
        var claimIdsToRemove = 
            currentUserClaimIdsOnThisAndAncestors
            .Except(request.FolderPermissionIds)
            .ToList();

        var targetUserFolderClaimsToRemove = 
            context.UserJobFolderClaims
            .Where(ujfc => ujfc.FolderId == request.FolderId 
                           && ujfc.UserId == request.UserId
                           && claimIdsToRemove.Contains(ujfc.ClaimId));
        
        context.UserJobFolderClaims.RemoveRange(targetUserFolderClaimsToRemove);
        context.UserJobFolderClaims.AddRange(request.FolderPermissionIds
            .Select(id => new UserJobFolderClaim(request.UserId, request.FolderId, id)));
        await context.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}
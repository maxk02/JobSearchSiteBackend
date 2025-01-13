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
        
        if (request.UserId == currentUserId)
            return Result.Error();
        
        var currentUserClaimIdsForThisAndAncestors = await context.JobFolderRelations
            .GetClaimIdsForThisAndAncestors(request.FolderId, currentUserId)
            .ToListAsync(cancellationToken);
        
        if (!currentUserClaimIdsForThisAndAncestors.Contains(JobFolderClaim.IsAdmin.Id))
            return Result.Forbidden("Current user is not a folder admin.");
        
        if (request.JobFolderClaimIds.Except(currentUserClaimIdsForThisAndAncestors).Any())
            return Result.Forbidden();
        
        var targetUserClaimIdsForThisAndAncestors = await context.JobFolderRelations
            .GetClaimIdsForThisAndAncestors(request.FolderId, request.UserId)
            .ToListAsync(cancellationToken);
        
        if (targetUserClaimIdsForThisAndAncestors.Contains(JobFolderClaim.IsOwner.Id))
            return Result.Forbidden();
        
        if (targetUserClaimIdsForThisAndAncestors.Contains(JobFolderClaim.IsAdmin.Id) &&
            !currentUserClaimIdsForThisAndAncestors.Contains(JobFolderClaim.IsOwner.Id))
            return Result.Forbidden();
        
        var targetUserClaimIdsForThis = await context.UserJobFolderClaims
            .Where(ujfc => ujfc.UserId == request.UserId)
            .Where(ujfc => ujfc.FolderId == request.FolderId)
            .Select(ujfc => ujfc.ClaimId)
            .ToListAsync(cancellationToken);

        var claimIdsForAncestors =
            targetUserClaimIdsForThisAndAncestors
                .Except(targetUserClaimIdsForThis)
                .ToList();

        var claimIdsThatTargetUserAlreadyHasAtAncestors =
            claimIdsForAncestors
                .Intersect(request.JobFolderClaimIds);
        
        if (claimIdsThatTargetUserAlreadyHasAtAncestors.Any())
            return Result.Error();

        var claimIdsForAncestorsWithRequestedClaimsConcated = 
            claimIdsForAncestors
                .Concat(request.JobFolderClaimIds)
                .ToList();
        
        var permissionsValidator = new JobFolderClaimIdCollectionValidator();
        var validationResult = await permissionsValidator
            .ValidateAsync(claimIdsForAncestorsWithRequestedClaimsConcated, cancellationToken);
        if (!validationResult.IsValid)
            return Result.Invalid(validationResult.AsErrors());
        
        var claimIdsToRemove = 
            currentUserClaimIdsForThisAndAncestors
            .Except(request.JobFolderClaimIds)
            .ToList();

        var targetUserFolderClaimsToRemove = 
            await context.UserJobFolderClaims
            .Where(ujfc => ujfc.FolderId == request.FolderId 
                           && ujfc.UserId == request.UserId
                           && claimIdsToRemove.Contains(ujfc.ClaimId))
            .ToListAsync(cancellationToken);
        
        context.UserJobFolderClaims.RemoveRange(targetUserFolderClaimsToRemove);
        context.UserJobFolderClaims.AddRange(request.JobFolderClaimIds
            .Select(id => new UserJobFolderClaim(request.UserId, request.FolderId, id)));
        await context.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.JobFolders;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using JobSearchSiteBackend.Core.Domains.JobFolders.Persistence;
using JobSearchSiteBackend.Core.Persistence;

namespace JobSearchSiteBackend.Core.Domains.JobFolderClaims.UseCases.UpdateJobFolderClaimIdsForUser;

public class UpdateJobFolderClaimIdsForUserHandler(ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<UpdateJobFolderClaimIdsForUserCommand, Result>
{
    public async Task<Result> Handle(UpdateJobFolderClaimIdsForUserCommand command,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();
        
        if (command.UserId == currentUserId)
            return Result.Error();
        
        var currentUserClaimIdsForThisAndAncestors = await context.JobFolderRelations
            .GetClaimIdsForThisAndAncestors(command.JobFolderId, currentUserId)
            .ToListAsync(cancellationToken);
        
        if (!currentUserClaimIdsForThisAndAncestors.Contains(JobFolderClaim.IsAdmin.Id))
            return Result.Forbidden("Current user is not a folder admin.");
        
        if (command.JobFolderClaimIds.Except(currentUserClaimIdsForThisAndAncestors).Any())
            return Result.Forbidden();
        
        var targetUserClaimIdsForThisAndAncestors = await context.JobFolderRelations
            .GetClaimIdsForThisAndAncestors(command.JobFolderId, command.UserId)
            .ToListAsync(cancellationToken);
        
        if (targetUserClaimIdsForThisAndAncestors.Contains(JobFolderClaim.IsOwner.Id))
            return Result.Forbidden();
        
        if (targetUserClaimIdsForThisAndAncestors.Contains(JobFolderClaim.IsAdmin.Id) &&
            !currentUserClaimIdsForThisAndAncestors.Contains(JobFolderClaim.IsOwner.Id))
            return Result.Forbidden();
        
        var targetUserClaimIdsForThis = await context.UserJobFolderClaims
            .Where(ujfc => ujfc.UserId == command.UserId)
            .Where(ujfc => ujfc.FolderId == command.JobFolderId)
            .Select(ujfc => ujfc.ClaimId)
            .ToListAsync(cancellationToken);

        var claimIdsForAncestors =
            targetUserClaimIdsForThisAndAncestors
                .Except(targetUserClaimIdsForThis)
                .ToList();

        var claimIdsThatTargetUserAlreadyHasAtAncestors =
            claimIdsForAncestors
                .Intersect(command.JobFolderClaimIds);
        
        if (claimIdsThatTargetUserAlreadyHasAtAncestors.Any())
            return Result.Error();

        var claimIdsForAncestorsWithRequestedClaimsConcated = 
            claimIdsForAncestors
                .Concat(command.JobFolderClaimIds)
                .ToList();
        
        var permissionsValidator = new JobFolderClaimIdCollectionValidator();
        var validationResult = await permissionsValidator
            .ValidateAsync(claimIdsForAncestorsWithRequestedClaimsConcated, cancellationToken);
        if (!validationResult.IsValid)
            return Result.Invalid(validationResult.AsErrors());
        
        var claimIdsToRemove = 
            currentUserClaimIdsForThisAndAncestors
            .Except(command.JobFolderClaimIds)
            .ToList();

        var targetUserFolderClaimsToRemove = 
            await context.UserJobFolderClaims
            .Where(ujfc => ujfc.FolderId == command.JobFolderId 
                           && ujfc.UserId == command.UserId
                           && claimIdsToRemove.Contains(ujfc.ClaimId))
            .ToListAsync(cancellationToken);
        
        context.UserJobFolderClaims.RemoveRange(targetUserFolderClaimsToRemove);
        context.UserJobFolderClaims.AddRange(command.JobFolderClaimIds
            .Select(id => new UserJobFolderClaim(command.UserId, command.JobFolderId, id)));
        await context.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}
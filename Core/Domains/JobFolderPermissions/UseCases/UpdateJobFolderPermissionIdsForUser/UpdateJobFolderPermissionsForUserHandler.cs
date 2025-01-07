using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.JobFolderPermissions.UserJobFolderPermissions;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Shared.Result;
using Shared.Result.FluentValidation;

namespace Core.Domains.JobFolderPermissions.UseCases.UpdateJobFolderPermissionIdsForUser;

public class UpdateJobFolderPermissionsForUserHandler(ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<UpdateJobFolderPermissionIdsForUserRequest, Result>
{
    public async Task<Result> Handle(UpdateJobFolderPermissionIdsForUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var permissionsValidator = new JobFolderPermissionIdCollectionValidator();
        var validationResult = permissionsValidator.Validate(request.FolderPermissionIds);
        if (!validationResult.IsValid)
            return Result.Invalid(validationResult.AsErrors());
        
        var currentUserPermissionIds = await context.UserJobFolderPermissions
            .Where(ujfp => ujfp.UserId == currentUserId && ujfp.FolderId == request.FolderId)
            .Select(ujfp => ujfp.PermissionId)
            .ToListAsync(cancellationToken);

        if (!currentUserPermissionIds.Contains(JobFolderPermission.IsAdmin.Id))
            return Result.Forbidden("Current user is not a folder admin.");
        
        var targetUserJobFolderPermissions = await context.UserJobFolderPermissions
            .Where(ujfp => ujfp.UserId == request.UserId && ujfp.FolderId == request.FolderId)
            .ToListAsync(cancellationToken);
        
        if (targetUserJobFolderPermissions.Select(ujfp => ujfp.PermissionId).Contains(JobFolderPermission.IsAdmin.Id))
            return Result.Forbidden("Insufficient permissions for update of permissions of target user.");
        
        if (targetUserJobFolderPermissions.Select(ujfp => ujfp.PermissionId).Except(currentUserPermissionIds).Any())
            return Result.Forbidden("Insufficient permissions for update of permissions of target user.");
        
        context.UserJobFolderPermissions.RemoveRange(targetUserJobFolderPermissions);
        context.UserJobFolderPermissions.AddRange(request.FolderPermissionIds
            .Select(id => new UserJobFolderPermission(request.UserId, request.FolderId, id)));
        await context.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}
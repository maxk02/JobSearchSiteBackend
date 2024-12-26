using Core.Domains._Shared.UseCaseStructure;
using Core.Services.Auth.Authentication;
using Shared.Result;
using Shared.Result.FluentValidation;

namespace Core.Domains.JobFolderPermissions.UseCases.UpdateJobFolderPermissionIdsForUser;

public class UpdateJobFolderPermissionsForUserHandler(
    ICurrentAccountService currentAccountService,
    IJobFolderPermissionRepository jobFolderPermissionRepository) 
    : IRequestHandler<UpdateJobFolderPermissionIdsForUserRequest, Result>
{
    public async Task<Result> Handle(UpdateJobFolderPermissionIdsForUserRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var permissionsValidator = new JobFolderPermissionIdCollectionValidator();
        var validationResult = permissionsValidator.Validate(request.FolderPermissionIds);
        if (!validationResult.IsValid)
            return Result.Invalid(validationResult.AsErrors());
        
        var currentUserPermissions = await jobFolderPermissionRepository
            .GetPermissionIdsForUserAsync(currentUserId, request.FolderId, cancellationToken);

        if (!currentUserPermissions.Contains(JobFolderPermission.IsAdmin.Id))
            return Result.Forbidden("Current user is not a folder admin.");
        
        var targetUserPermissions = await jobFolderPermissionRepository
            .GetPermissionIdsForUserAsync(request.UserId, request.FolderId, cancellationToken);
        
        if (targetUserPermissions.Contains(JobFolderPermission.IsOwner.Id) || targetUserPermissions.Contains(JobFolderPermission.IsAdmin.Id))
            return Result.Forbidden("Insufficient permissions for update of permissions of target user.");
        
        if (targetUserPermissions.Except(currentUserPermissions).Any())
            return Result.Forbidden("Insufficient permissions for update of permissions of target user.");
        
        await jobFolderPermissionRepository.UpdatePermissionIdsForUserAsync(currentUserId, request.FolderId,
            request.FolderPermissionIds, cancellationToken);
        
        return Result.Success();
    }
}
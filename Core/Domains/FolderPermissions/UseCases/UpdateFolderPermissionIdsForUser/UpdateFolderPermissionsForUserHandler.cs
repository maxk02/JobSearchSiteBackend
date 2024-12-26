using Core.Domains._Shared.UseCaseStructure;
using Core.Services.Auth.Authentication;
using Shared.Result;
using Shared.Result.FluentValidation;

namespace Core.Domains.FolderPermissions.UseCases.UpdateFolderPermissionIdsForUser;

public class UpdateFolderPermissionsForUserHandler(
    ICurrentAccountService currentAccountService,
    IFolderPermissionRepository folderPermissionRepository) 
    : IRequestHandler<UpdateFolderPermissionIdsForUserRequest, Result>
{
    public async Task<Result> Handle(UpdateFolderPermissionIdsForUserRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var permissionsValidator = new FolderPermissionIdCollectionValidator();
        var validationResult = permissionsValidator.Validate(request.FolderPermissionIds);
        if (!validationResult.IsValid)
            return Result.Invalid(validationResult.AsErrors());
        
        var currentUserPermissions = await folderPermissionRepository
            .GetPermissionIdsForUserAsync(currentUserId, request.FolderId, cancellationToken);

        if (!currentUserPermissions.Contains(FolderPermission.IsAdmin.Id))
            return Result.Forbidden("Current user is not a folder admin.");
        
        var targetUserPermissions = await folderPermissionRepository
            .GetPermissionIdsForUserAsync(request.UserId, request.FolderId, cancellationToken);
        
        if (targetUserPermissions.Contains(FolderPermission.IsOwner.Id) || targetUserPermissions.Contains(FolderPermission.IsAdmin.Id))
            return Result.Forbidden("Insufficient permissions for update of permissions of target user.");
        
        if (targetUserPermissions.Except(currentUserPermissions).Any())
            return Result.Forbidden("Insufficient permissions for update of permissions of target user.");
        
        await folderPermissionRepository.UpdatePermissionIdsForUserAsync(currentUserId, request.FolderId,
            request.FolderPermissionIds, cancellationToken);
        
        return Result.Success();
    }
}
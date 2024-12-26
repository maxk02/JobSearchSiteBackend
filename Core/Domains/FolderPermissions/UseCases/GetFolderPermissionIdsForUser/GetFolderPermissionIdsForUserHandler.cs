using Core.Domains._Shared.UseCaseStructure;
using Core.Services.Auth.Authentication;
using Shared.Result;

namespace Core.Domains.FolderPermissions.UseCases.GetFolderPermissionIdsForUser;

public class GetFolderPermissionIdsForUserHandler(
    ICurrentAccountService currentAccountService,
    IFolderPermissionRepository folderPermissionRepository)
    : IRequestHandler<GetFolderPermissionIdsForUserRequest, Result<ICollection<long>>>
{
    public async Task<Result<ICollection<long>>> Handle(GetFolderPermissionIdsForUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        if (currentUserId != request.UserId)
        {
            var isAdmin = await folderPermissionRepository.HasPermissionIdAsync(currentUserId,
                request.FolderId, FolderPermission.IsAdmin.Id, cancellationToken);
            
            if (!isAdmin) 
                return Result<ICollection<long>>.Forbidden();
        }
        
        var permissions = await folderPermissionRepository
            .GetPermissionIdsForUserAsync(request.UserId, request.FolderId, cancellationToken);

        return Result<ICollection<long>>.Success(permissions);
    }
}
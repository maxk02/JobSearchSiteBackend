using Core.Domains._Shared.UseCaseStructure;
using Core.Services.Auth.Authentication;
using Shared.Result;

namespace Core.Domains.JobFolderPermissions.UseCases.GetJobFolderPermissionIdsForUser;

public class GetJobFolderPermissionIdsForUserHandler(
    ICurrentAccountService currentAccountService,
    IJobFolderPermissionRepository jobFolderPermissionRepository)
    : IRequestHandler<GetJobFolderPermissionIdsForUserRequest, Result<ICollection<long>>>
{
    public async Task<Result<ICollection<long>>> Handle(GetJobFolderPermissionIdsForUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        if (currentUserId != request.UserId)
        {
            var isAdmin = await jobFolderPermissionRepository.HasPermissionIdAsync(currentUserId,
                request.FolderId, JobFolderPermission.IsAdmin.Id, cancellationToken);
            
            if (!isAdmin) 
                return Result<ICollection<long>>.Forbidden();
        }
        
        var permissions = await jobFolderPermissionRepository
            .GetPermissionIdsForUserAsync(request.UserId, request.FolderId, cancellationToken);

        return Result<ICollection<long>>.Success(permissions);
    }
}
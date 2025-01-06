using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.CompanyPermissions;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Shared.Result;

namespace Core.Domains.JobFolderPermissions.UseCases.GetJobFolderPermissionIdsForUser;

public class GetJobFolderPermissionIdsForUserHandler(IJwtCurrentAccountService jwtCurrentAccountService,
    MainDataContext context) : IRequestHandler<GetJobFolderPermissionIdsForUserRequest, Result<ICollection<long>>>
{
    public async Task<Result<ICollection<long>>> Handle(GetJobFolderPermissionIdsForUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = jwtCurrentAccountService.GetIdOrThrow();

        if (currentUserId != request.UserId)
        {
            //todo hierarchy
            var isAdmin = await context.UserJobFolderPermissions
                .AnyAsync(ujfp => ujfp.UserId == currentUserId 
                          && ujfp.FolderId == request.FolderId
                          && ujfp.PermissionId == CompanyPermission.IsAdmin.Id, cancellationToken);
            
            if (!isAdmin) 
                return Result<ICollection<long>>.Forbidden();
        }
        
        var permissionIds = await context.UserJobFolderPermissions
            .Where(ujfp => ujfp.UserId == request.UserId && ujfp.FolderId == request.FolderId)
            .Select(ujfp => ujfp.PermissionId)
            .ToListAsync(cancellationToken);

        return Result<ICollection<long>>.Success(permissionIds);
    }
}
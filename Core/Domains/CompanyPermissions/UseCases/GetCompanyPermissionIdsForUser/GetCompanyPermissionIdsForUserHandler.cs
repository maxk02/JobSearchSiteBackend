using Core.Domains._Shared.UseCaseStructure;
using Core.Services.Auth.Authentication;
using Shared.Result;

namespace Core.Domains.CompanyPermissions.UseCases.GetCompanyPermissionIdsForUser;

public class GetCompanyPermissionIdsForUserHandler(
    ICurrentAccountService currentAccountService,
    ICompanyPermissionRepository companyPermissionRepository)
    : IRequestHandler<GetCompanyPermissionIdsForUserRequest, Result<ICollection<long>>>
{
    public async Task<Result<ICollection<long>>> Handle(GetCompanyPermissionIdsForUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        if (currentUserId != request.UserId)
        {
            var isAdmin = await companyPermissionRepository.HasPermissionIdAsync(currentUserId,
                request.CompanyId, CompanyPermission.IsAdmin.Id, cancellationToken);
            
            if (!isAdmin) 
                return Result<ICollection<long>>.Forbidden();
        }
        
        var permissions = await companyPermissionRepository
            .GetPermissionIdsForUserAsync(request.UserId, request.CompanyId, cancellationToken);

        return Result<ICollection<long>>.Success(permissions);
    }
}
using Core.Domains._Shared.UseCaseStructure;
using Core.Persistence.EfCore;
using Core.Services.Auth.Authentication;
using Microsoft.EntityFrameworkCore;
using Shared.Result;

namespace Core.Domains.CompanyPermissions.UseCases.GetCompanyPermissionIdsForUser;

public class GetCompanyPermissionIdsForUserHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context)
    : IRequestHandler<GetCompanyPermissionIdsForUserRequest, Result<ICollection<long>>>
{
    public async Task<Result<ICollection<long>>> Handle(GetCompanyPermissionIdsForUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var targetUserPermissions = await context.UserCompanyPermissions
            .Where(ucp => ucp.UserId == currentUserId && ucp.CompanyId == request.CompanyId)
            .Select(ucp => ucp.Id)
            .ToListAsync(cancellationToken);
        
        if (currentUserId == request.UserId)
        {
            return Result<ICollection<long>>.Success(targetUserPermissions);
        }
        
        var isCurrentUserAdmin = await context.UserCompanyPermissions.AnyAsync(
            ucp => ucp.UserId == currentUserId
                   && ucp.CompanyId == request.CompanyId
                   && ucp.Id == CompanyPermission.IsAdmin.Id,
            cancellationToken);
            
        if (!isCurrentUserAdmin) 
            return Result<ICollection<long>>.Forbidden();
        
        return Result<ICollection<long>>.Success(targetUserPermissions);
    }
}
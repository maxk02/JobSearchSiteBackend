using Core.Domains._Shared.UseCaseStructure;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Shared.Result;

namespace Core.Domains.CompanyClaims.UseCases.GetCompanyClaimIdsForUser;

public class GetCompanyClaimIdsForUserHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context)
    : IRequestHandler<GetCompanyClaimIdsForUserRequest, Result<ICollection<long>>>
{
    public async Task<Result<ICollection<long>>> Handle(GetCompanyClaimIdsForUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var targetUserPermissions = await context.UserCompanyClaims
            .Where(ucp => ucp.UserId == currentUserId && ucp.CompanyId == request.CompanyId)
            .Select(ucp => ucp.Id)
            .ToListAsync(cancellationToken);
        
        if (currentUserId == request.UserId)
        {
            return Result<ICollection<long>>.Success(targetUserPermissions);
        }
        
        var isCurrentUserAdmin = await context.UserCompanyClaims.AnyAsync(
            ucp => ucp.UserId == currentUserId
                   && ucp.CompanyId == request.CompanyId
                   && ucp.Id == CompanyClaim.IsAdmin.Id,
            cancellationToken);
            
        if (!isCurrentUserAdmin) 
            return Result<ICollection<long>>.Forbidden();
        
        return Result<ICollection<long>>.Success(targetUserPermissions);
    }
}
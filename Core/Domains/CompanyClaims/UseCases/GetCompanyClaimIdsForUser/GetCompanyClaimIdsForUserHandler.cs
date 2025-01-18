using Core.Domains._Shared.UseCaseStructure;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;

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

        var currentUserClaimIds = await context.UserCompanyClaims
            .Where(ucp => ucp.UserId == currentUserId && ucp.CompanyId == request.CompanyId)
            .Select(ucp => ucp.Id)
            .ToListAsync(cancellationToken);
        
        if (request.UserId == currentUserId)
            return currentUserClaimIds;
        
        if (!currentUserClaimIds.Contains(CompanyClaim.IsAdmin.Id))
            return Result<ICollection<long>>.Forbidden();
        
        var targetUserClaimIds = await context.UserCompanyClaims
            .Where(ucp => ucp.UserId == request.UserId && ucp.CompanyId == request.CompanyId)
            .Select(ucp => ucp.Id)
            .ToListAsync(cancellationToken);

        var visibleClaimIds = currentUserClaimIds
            .Intersect(targetUserClaimIds)
            .ToList();

        return visibleClaimIds;
    }
}
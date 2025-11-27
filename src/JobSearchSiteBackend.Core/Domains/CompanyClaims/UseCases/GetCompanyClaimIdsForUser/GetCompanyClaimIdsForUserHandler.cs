using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;
using JobSearchSiteBackend.Core.Persistence;

namespace JobSearchSiteBackend.Core.Domains.CompanyClaims.UseCases.GetCompanyClaimIdsForUser;

public class GetCompanyClaimIdsForUserHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context)
    : IRequestHandler<GetCompanyClaimIdsForUserQuery, Result<GetCompanyClaimIdsForUserResult>>
{
    public async Task<Result<GetCompanyClaimIdsForUserResult>> Handle(GetCompanyClaimIdsForUserQuery query,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var currentUserClaimIds = await context.UserCompanyClaims
            .Where(ucp => ucp.UserId == currentUserId && ucp.CompanyId == query.CompanyId)
            .Select(ucp => ucp.Id)
            .ToListAsync(cancellationToken);
        
        if (query.UserId == currentUserId)
            return Result.Success(new GetCompanyClaimIdsForUserResult(currentUserClaimIds));
        
        if (!currentUserClaimIds.Contains(CompanyClaim.IsAdmin.Id))
            return Result.Forbidden();
        
        var targetUserClaimIds = await context.UserCompanyClaims
            .Where(ucp => ucp.UserId == query.UserId && ucp.CompanyId == query.CompanyId)
            .Select(ucp => ucp.ClaimId)
            .ToListAsync(cancellationToken);

        var visibleClaimIds = currentUserClaimIds
            .Intersect(targetUserClaimIds)
            .ToList();

        return Result.Success(new GetCompanyClaimIdsForUserResult(visibleClaimIds));
    }
}
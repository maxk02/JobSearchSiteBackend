using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.CompanyClaims.Dtos;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.CompanyClaims.UseCases.GetCompanyClaimsOverview;

public class GetCompanyClaimsOverviewHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context)
    : IRequestHandler<GetCompanyClaimsOverviewQuery, Result<GetCompanyClaimsOverviewResult>>
{
    public async Task<Result<GetCompanyClaimsOverviewResult>> Handle(GetCompanyClaimsOverviewQuery query,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var currentUserClaimIds = await context.UserCompanyClaims
            .Where(ucp => ucp.UserId == currentUserId && ucp.CompanyId == query.CompanyId)
            .Select(ucp => ucp.Id)
            .ToListAsync(cancellationToken);

        if (!currentUserClaimIds.Contains(CompanyClaim.IsAdmin.Id))
            return Result.Forbidden();

        var dbQuery = context.UserCompanyClaims
            .Where(ucp => ucp.CompanyId == query.CompanyId);
        
        var count = await dbQuery.CountAsync(cancellationToken);

        if (!string.IsNullOrEmpty(query.UserQuery))
        {
            dbQuery = dbQuery
                .Where(ucp => ucp.User!.FirstName.ToLower().Contains(query.UserQuery.ToLower())
                || ucp.User!.LastName.ToLower().Contains(query.UserQuery.ToLower())
                || ucp.User!.Account!.Email!.ToLower().Contains(query.UserQuery.ToLower()));
        }

        if (query.CompanyClaimIds is not null && query.CompanyClaimIds.Count > 0)
        {
            dbQuery = dbQuery
                .Where(ucp => query.CompanyClaimIds.Contains(ucp.ClaimId));
        }
        
        dbQuery = dbQuery
            .Skip(query.Size * (query.Page - 1))
            .Take(query.Size);

        var dtoDbQuery = dbQuery
            .Select(ucp => new CompanyClaimOverviewDto(
                ucp.Id,
                ucp.UserId,
                ucp.User!.FirstName,
                ucp.User!.LastName,
                ucp.User!.Account!.Email!,
                ucp.ClaimId
            ));

        var dtoDbQueryResult = await dtoDbQuery.ToListAsync(cancellationToken);
        var paginationResponse = new PaginationResponse(query.Page, query.Size, count);

        var result = new GetCompanyClaimsOverviewResult(dtoDbQueryResult, paginationResponse);
        
        return Result.Success(result);
    }
}
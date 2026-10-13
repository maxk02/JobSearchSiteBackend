using Ardalis.Result;
using AutoMapper;
using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Companies.Dtos;
using JobSearchSiteBackend.Core.Domains.CompanyClaims;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyBalanceTransactions;

public class GetCompanyBalanceTransactionsHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context)
    : IRequestHandler<GetCompanyBalanceTransactionsQuery, Result<GetCompanyBalanceTransactionsResult>>
{
    public async Task<Result<GetCompanyBalanceTransactionsResult>> Handle(GetCompanyBalanceTransactionsQuery query,
        CancellationToken cancellationToken = default)
    {
        var currentAccountId = currentAccountService.GetIdOrThrow();

        var hasPermission = await context.UserCompanyClaims
            .Where(ucc => ucc.CompanyId == query.CompanyId
                          && ucc.UserId == currentAccountId
                          && ucc.ClaimId == CompanyClaim.CanManageBalance.Id)
            .AnyAsync(cancellationToken);

        if (!hasPermission)
        {
            return Result.Forbidden();
        }

        var dbQuery = context.CompanyBalanceTransactions
            .Where(cbt => cbt.CompanyId == query.CompanyId);

        var totalCount = await dbQuery.CountAsync(cancellationToken);
        
        var companyBalanceTransactions = await dbQuery
            .OrderByDescending(cbt => cbt.DateTimeCommittedUtc)
            .Skip((query.Page - 1) * query.Size)
            .Take(query.Size)
            .Select(cbt => new CompanyBalanceTransactionDto(cbt.Id, cbt.DateTimeCommittedUtc, cbt.UserProfileId,
                cbt.UserProfile!.FirstName + " " + cbt.UserProfile!.LastName, cbt.UserProfile!.Account!.Email,
                cbt.Amount, cbt.CurrencyId, cbt.Description))
            .ToListAsync(cancellationToken);

        if (companyBalanceTransactions.Count == 0)
            return Result.NotFound();
        
        var paginationResponse = new PaginationResponse(query.Page, query.Size, totalCount);

        var result = new GetCompanyBalanceTransactionsResult(companyBalanceTransactions, paginationResponse);

        return Result.Success(result);
    }
}
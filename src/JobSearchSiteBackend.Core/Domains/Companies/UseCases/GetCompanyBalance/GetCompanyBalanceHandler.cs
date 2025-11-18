using Ardalis.Result;
using AutoMapper;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Companies.Dtos;
using JobSearchSiteBackend.Core.Domains.CompanyClaims;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyBalance;

public class GetCompanyBalanceHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context)
    : IRequestHandler<GetCompanyBalanceRequest, Result<GetCompanyBalanceResponse>>
{
    public async Task<Result<GetCompanyBalanceResponse>> Handle(GetCompanyBalanceRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentAccountId = currentAccountService.GetIdOrThrow();

        var hasPermission = await context.UserCompanyClaims
            .Where(ucc => ucc.CompanyId == request.Id
                          && ucc.UserId == currentAccountId
                          && ucc.ClaimId == CompanyClaim.CanManageBalance.Id)
            .AnyAsync(cancellationToken);

        if (!hasPermission)
        {
            return Result.Forbidden();
        }
        
        var companyWithBalance = await context.Companies
            .Where(c => c.Id == request.Id)
            .Select(c => new { Company = c, Balance = c.CompanyBalanceTransactions!.Select(cbt => cbt.Amount).Sum() })
            .SingleOrDefaultAsync(cancellationToken);

        if (companyWithBalance is null)
            return Result<GetCompanyBalanceResponse>.NotFound();
        
        
        return new GetCompanyBalanceResponse("PLN",  companyWithBalance.Balance);
    }
}
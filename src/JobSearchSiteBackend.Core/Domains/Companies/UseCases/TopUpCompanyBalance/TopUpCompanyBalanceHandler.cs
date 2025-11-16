using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.CompanyClaims;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.TopUpCompanyBalance;

public class TopUpCompanyBalanceHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<TopUpCompanyBalanceRequest, Result>
{
    public async Task<Result> Handle(TopUpCompanyBalanceRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var companyWithPermissionIdsQuery =
            context.Companies
                .Select(c => new
                {
                    Company = c,
                    HasPermission = c.UserCompanyClaims!
                        .Any(ucc => ucc.ClaimId == CompanyClaim.CanManageBalance.Id && ucc.UserId == currentUserId),
                });

        var companyWithClaimIds = await companyWithPermissionIdsQuery.SingleOrDefaultAsync(cancellationToken);

        if (companyWithClaimIds is null)
            return Result.NotFound();

        if (!companyWithClaimIds.HasPermission)
        {
            return Result.Forbidden();
        }

        var companyBalanceTransaction = new CompanyBalanceTransaction(request.Id, request.Amount,
            request.CurrencyCode, currentUserId);

        context.CompanyBalanceTransactions.Add(companyBalanceTransaction);
        
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
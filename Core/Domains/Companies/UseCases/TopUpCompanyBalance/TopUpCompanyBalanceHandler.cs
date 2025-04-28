using Ardalis.Result;
using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.Companies.Search;
using Core.Domains.CompanyClaims;
using Core.Persistence;
using Core.Services.Auth;
using Core.Services.BackgroundJobs;
using Microsoft.EntityFrameworkCore;

namespace Core.Domains.Companies.UseCases.UpdateCompanyBalance;

public class TopUpCompanyBalanceHandler(
    ICurrentAccountService currentAccountService,
    ICompanySearchRepository companySearchRepository,
    IBackgroundJobService backgroundJobService,
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

        var company = companyWithClaimIds.Company;

        company.Balance += request.BalanceUpdate;

        context.Companies.Update(company);
        
        
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
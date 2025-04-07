using Ardalis.Result;
using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.Companies.Search;
using Core.Domains.CompanyClaims;
using Core.Persistence;
using Core.Services.Auth;
using Core.Services.BackgroundJobs;
using Microsoft.EntityFrameworkCore;

namespace Core.Domains.Companies.UseCases.UpdateCompanyBalance;

public class UpdateCompanyBalanceHandler(
    ICurrentAccountService currentAccountService,
    ICompanySearchRepository companySearchRepository,
    IBackgroundJobService backgroundJobService,
    MainDataContext context) : IRequestHandler<UpdateCompanyBalanceRequest, Result>
{
    public async Task<Result> Handle(UpdateCompanyBalanceRequest request, CancellationToken cancellationToken = default)
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
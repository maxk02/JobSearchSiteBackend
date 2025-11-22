using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.CompanyClaims;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.AddCompanyEmployee;

public class AddCompanyEmployeeHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context)
    : IRequestHandler<AddCompanyEmployeeCommand, Result>
{
    public async Task<Result> Handle(AddCompanyEmployeeCommand command,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();
        
        var companyWithPermissionIdsQuery =
            from dbCompany in context.Companies
            join dbUcc in context.UserCompanyClaims on dbCompany.Id equals dbUcc.CompanyId into ucpGroup
            from dbUcc in ucpGroup.DefaultIfEmpty()
            where dbCompany.Id == command.CompanyId && dbUcc.UserId == currentUserId
            group dbUcc.ClaimId by new { Company = dbCompany, UserId = dbUcc.UserId }
            into grouped
            select new { grouped.Key.Company, PermissionIds = grouped.ToList() };

        var companyWithClaimIds = await companyWithPermissionIdsQuery.SingleOrDefaultAsync(cancellationToken);

        if (companyWithClaimIds is null)
            return Result.NotFound();

        if (!companyWithClaimIds.PermissionIds.Contains(CompanyClaim.IsAdmin.Id))
        {
            return Result.Forbidden();
        }

        var userProfile = await context.UserProfiles
            .Include(u => u.CompaniesWhereEmployed)
            .Where(u => u.Id == currentUserId)
            .SingleOrDefaultAsync(cancellationToken);

        if (userProfile is null)
        {
            return Result.Error();
        }
        
        userProfile.CompaniesWhereEmployed!.Add(companyWithClaimIds.Company);
        context.UserProfiles.Update(userProfile);
        
        // saving changes
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
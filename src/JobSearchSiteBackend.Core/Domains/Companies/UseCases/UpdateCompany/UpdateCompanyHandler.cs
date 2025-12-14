using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Companies.Search;
using JobSearchSiteBackend.Core.Domains.CompanyClaims;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;
using JobSearchSiteBackend.Core.Persistence;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.UpdateCompany;

public class UpdateCompanyHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<UpdateCompanyCommand, Result>
{
    public async Task<Result> Handle(UpdateCompanyCommand command, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var companyWithPermissionIdsQuery =
            from dbCompany in context.Companies
            join dbUcc in context.UserCompanyClaims on dbCompany.Id equals dbUcc.CompanyId into ucpGroup
            from dbUcc in ucpGroup.DefaultIfEmpty()
            where dbCompany.Id == command.Id && dbUcc.UserId == currentUserId
            group dbUcc.ClaimId by new { Company = dbCompany, UserId = dbUcc.UserId }
            into grouped
            select new { grouped.Key.Company, PermissionIds = grouped.ToList() };

        var companyWithClaimIds = await companyWithPermissionIdsQuery.SingleOrDefaultAsync(cancellationToken);

        if (companyWithClaimIds is null)
            return Result.NotFound();

        if (!companyWithClaimIds.PermissionIds.Contains(CompanyClaim.CanEditProfile.Id))
        {
            return Result.Forbidden();
        }

        var company = companyWithClaimIds.Company;

        if (command.Name is not null) company.Name = command.Name;
        if (command.Description is not null) company.Description = command.Description;
        if (command.IsPublic is not null) company.IsPublic = command.IsPublic.Value;

        var companySearchModel = new CompanySearchModel(
            company.Id,
            company.CountryId,
            company.Name,
            company.Description,
            new DateTime(), //company.DateTimeUpdatedUtc
            false //company.isDeleted
        );

        context.Companies.Update(company);
        
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
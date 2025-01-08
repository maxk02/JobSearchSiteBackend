using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.Companies.Search;
using Core.Domains.CompanyClaims;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Core.Services.BackgroundJobs;
using Microsoft.EntityFrameworkCore;
using Shared.Result;

namespace Core.Domains.Companies.UseCases.UpdateCompany;

public class UpdateCompanyHandler(
    ICurrentAccountService currentAccountService,
    ICompanySearchRepository companySearchRepository,
    IBackgroundJobService backgroundJobService,
    MainDataContext context) : IRequestHandler<UpdateCompanyRequest, Result>
{
    public async Task<Result> Handle(UpdateCompanyRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();
        
        var companyWithPermissionIdsQuery =
            from dbCompany in context.Companies
            join dbUcc in context.UserCompanyClaims on dbCompany.Id equals dbUcc.CompanyId into ucpGroup
            from dbUcc in ucpGroup.DefaultIfEmpty()
            where dbCompany.Id == request.CompanyId && dbUcc.UserId == currentUserId
            group dbUcc.PermissionId by new { Company = dbCompany, UserId = dbUcc.UserId } into grouped
            select new { grouped.Key.Company, PermissionIds = grouped.ToList() };

        var companyWithClaimIds = await companyWithPermissionIdsQuery.SingleOrDefaultAsync(cancellationToken);

        if (companyWithClaimIds is null)
            return Result.NotFound();
        
        if (!companyWithClaimIds.PermissionIds.Contains(CompanyClaim.CanEditProfile.Id))
        {
            return Result.Forbidden();
        }
        
        var company = companyWithClaimIds.Company;
        
        if (request.Name is not null) company.Name = request.Name;
        if (request.Description is not null) company.Description = request.Description;
        if (request.IsPublic is not null) company.IsPublic = request.IsPublic.Value;
        
        var companySearchModel = new CompanySearchModel(company.Id, company.CountryId,
            company.Name, company.Description);

        context.Companies.Update(company);
        await context.SaveChangesAsync(cancellationToken);
        
        backgroundJobService
            .Enqueue(() => companySearchRepository.UpdateAsync(companySearchModel, CancellationToken.None),
                BackgroundJobQueues.CompanySearch);
        
        return Result.Success();
    }
}
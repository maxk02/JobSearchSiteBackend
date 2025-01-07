using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.Companies.Search;
using Core.Domains.CompanyPermissions;
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
            from company in context.Companies
            join ucp in context.UserCompanyPermissions on company.Id equals ucp.CompanyId into ucpGroup
            from ucp in ucpGroup.DefaultIfEmpty()
            where company.Id == request.CompanyId && ucp.UserId == currentUserId
            group ucp.PermissionId by new { Company = company, UserId = ucp.UserId } into grouped
            select new { grouped.Key.Company, PermissionIds = grouped.ToList() };

        var companyWithPermissionIds = await companyWithPermissionIdsQuery.SingleOrDefaultAsync(cancellationToken);

        if (companyWithPermissionIds is null)
            return Result.NotFound();
        
        if (!companyWithPermissionIds.PermissionIds.Contains(CompanyPermission.CanEditProfile.Id))
        {
            return Result.Forbidden();
        }

        var updatedCompanyResult = Company.Create(
            request.Name ?? companyWithPermissionIds.Company.Name,
            request.Description ?? companyWithPermissionIds.Company.Description,
            request.IsPublic ?? companyWithPermissionIds.Company.IsPublic,
            companyWithPermissionIds.Company.CountryId,
            companyWithPermissionIds.Company.Id
        );
        
        if (updatedCompanyResult.IsFailure)
            return Result.WithMetadataFrom(updatedCompanyResult);
        
        var updatedCompany = updatedCompanyResult.Value;
        
        var companySearchModel = new CompanySearchModel(updatedCompany.Id, updatedCompany.CountryId,
            updatedCompany.Name, updatedCompany.Description);

        context.Companies.Update(updatedCompany);
        await context.SaveChangesAsync(cancellationToken);
        
        backgroundJobService
            .Enqueue(() => companySearchRepository.UpdateAsync(companySearchModel, CancellationToken.None),
                BackgroundJobQueues.CompanySearch);
        
        return Result.Success();
    }
}
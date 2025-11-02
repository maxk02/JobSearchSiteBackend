using System.Transactions;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Companies.Search;
using JobSearchSiteBackend.Core.Domains.CompanyClaims;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.BackgroundJobs;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;
using JobSearchSiteBackend.Core.Persistence;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.DeleteCompany;

public class DeleteCompanyHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context)
    : IRequestHandler<DeleteCompanyRequest, Result>
{
    public async Task<Result> Handle(DeleteCompanyRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var companyWithPermissionIdsQuery =
            from company in context.Companies
            join ucp in context.UserCompanyClaims on company.Id equals ucp.CompanyId into ucpGroup
            from ucp in ucpGroup.DefaultIfEmpty()
            where company.Id == request.Id && ucp.UserId == currentUserId
            group ucp.ClaimId by new { Company = company, UserId = ucp.UserId }
            into grouped
            select new { grouped.Key.Company, PermissionIds = grouped.ToList() };

        var companyWithPermissionIds = await companyWithPermissionIdsQuery.SingleOrDefaultAsync(cancellationToken);

        if (companyWithPermissionIds is null)
            return Result.NotFound();

        if (!companyWithPermissionIds.PermissionIds.Contains(CompanyClaim.IsOwner.Id))
            return Result.Forbidden("Insufficient permissions for requested company deletion.");

        context.Companies.Remove(companyWithPermissionIds.Company);

        var companyAvatars = await context.CompanyAvatars
            .Where(ca => ca.CompanyId == companyWithPermissionIds.Company.Id)
            .ToListAsync(cancellationToken);
        
        foreach (var companyAvatar in companyAvatars)
            companyAvatar.IsDeleted = true;

        context.CompanyAvatars.UpdateRange(companyAvatars);
        
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
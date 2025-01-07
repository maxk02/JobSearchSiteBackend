using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.Companies.Search;
using Core.Domains.CompanyPermissions;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Core.Services.BackgroundJobs;
using Microsoft.EntityFrameworkCore;
using Shared.Result;

namespace Core.Domains.Companies.UseCases.DeleteCompany;

public class DeleteCompanyHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context,
    ICompanySearchRepository companySearchRepository,
    IBackgroundJobService backgroundJobService)
    : IRequestHandler<DeleteCompanyRequest, Result>
{
    public async Task<Result> Handle(DeleteCompanyRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var companyWithPermissionIdsQuery =
            from company in context.Companies
            join ucp in context.UserCompanyPermissions on company.Id equals ucp.CompanyId into ucpGroup
            from ucp in ucpGroup.DefaultIfEmpty()
            where company.Id == request.CompanyId && ucp.UserId == currentUserId
            group ucp.PermissionId by new { Company = company, UserId = ucp.UserId }
            into grouped
            select new { grouped.Key.Company, PermissionIds = grouped.ToList() };

        var companyWithPermissionIds = await companyWithPermissionIdsQuery.SingleOrDefaultAsync(cancellationToken);

        if (companyWithPermissionIds is null)
            return Result.NotFound();

        if (!companyWithPermissionIds.PermissionIds.Contains(CompanyPermission.IsOwner.Id))
            return Result.Forbidden("Insufficient permissions for requested company deletion.");
        
        var companyId = companyWithPermissionIds.Company.Id;

        context.Companies.Remove(companyWithPermissionIds.Company);
        await context.SaveChangesAsync(cancellationToken);
        
        backgroundJobService.Enqueue(
            () => companySearchRepository.DeleteAsync(companyId, CancellationToken.None),
            BackgroundJobQueues.CompanySearch);

        return Result.Success();
    }
}
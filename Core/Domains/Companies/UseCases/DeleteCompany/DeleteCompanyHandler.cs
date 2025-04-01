using System.Transactions;
using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.Companies.Search;
using Core.Domains.CompanyClaims;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Core.Services.BackgroundJobs;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;

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
        
        var companyRowVersion = companyWithPermissionIds.Company.RowVersion;
        
        var companySearchModel = new CompanySearchModel(
            companyWithPermissionIds.Company.Id,
            companyWithPermissionIds.Company.CountryId,
            companyWithPermissionIds.Company.Name,
            companyWithPermissionIds.Company.Description
        );

        context.Companies.Remove(companyWithPermissionIds.Company);

        using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        
        await context.SaveChangesAsync(cancellationToken);
        
        backgroundJobService.Enqueue(
            () => companySearchRepository
                .SoftDeleteAsync(companySearchModel, companyRowVersion, CancellationToken.None),
            BackgroundJobQueues.CompanySearch);
        
        transaction.Complete();

        return Result.Success();
    }
}
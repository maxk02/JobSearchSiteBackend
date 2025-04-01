using System.Transactions;
using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.Companies.Search;
using Core.Domains.CompanyClaims;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Core.Services.BackgroundJobs;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;

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
            where dbCompany.Id == request.Id && dbUcc.UserId == currentUserId
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

        if (request.Name is not null) company.Name = request.Name;
        if (request.Description is not null) company.Description = request.Description;
        if (request.IsPublic is not null) company.IsPublic = request.IsPublic.Value;

        var companySearchModel = new CompanySearchModel(
            company.Id,
            company.CountryId,
            company.Name,
            company.Description
        );

        context.Companies.Update(company);

        using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        
        await context.SaveChangesAsync(cancellationToken);

        backgroundJobService
            .Enqueue(() => companySearchRepository
                    .AddOrUpdateIfNewestAsync(companySearchModel, company.RowVersion, CancellationToken.None),
                BackgroundJobQueues.CompanySearch);
        
        transaction.Complete();

        return Result.Success();
    }
}
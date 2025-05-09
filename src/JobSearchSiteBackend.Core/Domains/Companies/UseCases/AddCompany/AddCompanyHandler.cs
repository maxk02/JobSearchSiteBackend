using System.Transactions;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Companies.Search;
using JobSearchSiteBackend.Core.Domains.CompanyClaims;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims;
using JobSearchSiteBackend.Core.Domains.JobFolders;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.BackgroundJobs;
using Ardalis.Result;
using JobSearchSiteBackend.Core.Persistence;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.AddCompany;

public class AddCompanyHandler(
    ICurrentAccountService currentAccountService,
    ICompanySearchRepository companySearchRepository,
    IBackgroundJobService backgroundJobService,
    MainDataContext context)
    : IRequestHandler<AddCompanyRequest, Result<AddCompanyResponse>>
{
    public async Task<Result<AddCompanyResponse>> Handle(AddCompanyRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        //creating company and checking result
        var company = new Company(request.Name, request.Description, request.IsPublic, request.CountryId, request.LogoLink);

        var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        //adding company and saving early to retrieve generated id
        await context.Companies.AddAsync(company, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        var companySearchModel =
            new CompanySearchModel(company.Id, company.CountryId, company.Name, company.Description);

        //adding full permission set to user
        company.UserCompanyClaims = CompanyClaim.AllIds
            .Select(x => new UserCompanyClaim(currentUserId, company.Id, x)).ToList();


        //creating folder and checking result
        var rootFolder = new JobFolder(company.Id, null, null);

        //adding root folder to company
        await context.JobFolders.AddAsync(rootFolder, cancellationToken);

        //adding full permission set to user
        rootFolder.UserJobFolderClaims = JobFolderClaim.AllIds
            .Select(x => new UserJobFolderClaim(currentUserId, rootFolder.Id, x)).ToList();

        // saving changes
        await context.SaveChangesAsync(cancellationToken);

        backgroundJobService
            .Enqueue(
                () => companySearchRepository.
                    AddOrUpdateIfNewestAsync(companySearchModel, company.RowVersion, CancellationToken.None),
                BackgroundJobQueues.CompanySearch);
        
        //committing transaction
        transaction.Complete();

        return new AddCompanyResponse(company.Id);
    }
}
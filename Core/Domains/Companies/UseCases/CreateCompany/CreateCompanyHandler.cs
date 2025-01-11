using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.Companies.Search;
using Core.Domains.CompanyClaims;
using Core.Domains.JobFolderClaims;
using Core.Domains.JobFolders;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Core.Services.BackgroundJobs;
using Shared.Result;

namespace Core.Domains.Companies.UseCases.CreateCompany;

public class CreateCompanyHandler(
    ICurrentAccountService currentAccountService,
    ICompanySearchRepository companySearchRepository,
    IBackgroundJobService backgroundJobService,
    MainDataContext context)
    : IRequestHandler<CreateCompanyRequest, Result<CreateCompanyResponse>>
{
    public async Task<Result<CreateCompanyResponse>> Handle(CreateCompanyRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        //creating company and checking result
        var company = new Company(request.Name, request.Description, request.IsPublic, request.CountryId);
        
        //starting transaction to be able to use SaveChangesAsync multiple times and revert all changes if something fails
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        //adding company and saving early to retrieve generated id
        await context.Companies.AddAsync(company, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        
        var companySearchModel =
            new CompanySearchModel(company.Id, company.RowVersion, company.CountryId, company.Name, company.Description);

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

        //committing transaction
        await transaction.CommitAsync(cancellationToken);
        
        backgroundJobService
            .Enqueue(() => companySearchRepository.AddOrSetConstFieldsAsync(companySearchModel, CancellationToken.None),
            BackgroundJobQueues.CompanySearch);

        return new CreateCompanyResponse(company.Id);
    }
}
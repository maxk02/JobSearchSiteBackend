using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.Companies.Search;
using Core.Domains.CompanyPermissions;
using Core.Domains.CompanyPermissions.UserCompanyPermissions;
using Core.Domains.JobFolderPermissions;
using Core.Domains.JobFolderPermissions.UserJobFolderPermissions;
using Core.Domains.JobFolders;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Core.Services.BackgroundJobs;
using Shared.Result;

namespace Core.Domains.Companies.UseCases.CreateCompany;

public class CreateCompanyHandler(
    IJwtCurrentAccountService jwtCurrentAccountService,
    ICompanySearchRepository companySearchRepository,
    IBackgroundJobService backgroundJobService,
    MainDataContext context)
    : IRequestHandler<CreateCompanyRequest, Result<CreateCompanyResponse>>
{
    public async Task<Result<CreateCompanyResponse>> Handle(CreateCompanyRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = jwtCurrentAccountService.GetIdOrThrow();

        //creating company and checking result
        var companyCreationResult =
            Company.Create(request.Name, request.Description, request.IsPublic, request.CountryId);

        if (companyCreationResult.IsFailure)
            return Result<CreateCompanyResponse>.WithMetadataFrom(companyCreationResult);

        var company = companyCreationResult.Value;

        //starting transaction to be able to use SaveChangesAsync multiple times and revert all changes if something fails
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        //adding company and saving early to retrieve generated id
        await context.Companies.AddAsync(company, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        
        var companySearchModel =
            new CompanySearchModel(company.Id, company.CountryId, company.Name, company.Description);

        //adding full permission set to user
        company.UserCompanyPermissions = CompanyPermission.AllIds
            .Select(x => new UserCompanyPermission(currentUserId, company.Id, x)).ToList();


        //creating folder and checking result
        var rootFolderCreationResult = JobFolder.Create(company.Id, null, null);

        if (rootFolderCreationResult.IsFailure)
            return Result<CreateCompanyResponse>.Error();

        var rootFolder = rootFolderCreationResult.Value;


        //adding root folder to company
        await context.JobFolders.AddAsync(rootFolder, cancellationToken);

        //adding full permission set to user
        rootFolder.UserJobFolderPermissions = JobFolderPermission.AllIds
            .Select(x => new UserJobFolderPermission(currentUserId, rootFolder.Id, x)).ToList();

        // saving changes
        await context.SaveChangesAsync(cancellationToken);

        //committing transaction
        await transaction.CommitAsync(cancellationToken);
        
        backgroundJobService
            .Enqueue(() => companySearchRepository.AddAsync(companySearchModel, CancellationToken.None),
            BackgroundJobQueues.CompanySearch);

        return new CreateCompanyResponse(company.Id);
    }
}
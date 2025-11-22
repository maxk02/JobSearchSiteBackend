using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.CompanyClaims;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims;
using JobSearchSiteBackend.Core.Domains.JobFolders;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.AddCompany;

public class AddCompanyHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context)
    : IRequestHandler<AddCompanyCommand, Result<AddCompanyResult>>
{
    public async Task<Result<AddCompanyResult>> Handle(AddCompanyCommand command,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        //creating company and checking result
        var company = new Company(command.Name, command.Description, true, command.CountryId);

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        //adding company and saving early to retrieve generated id
        await context.Companies.AddAsync(company, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        // flagging company for update
        context.Companies.Update(company);
        
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

        return new AddCompanyResult(company.Id);
    }
}
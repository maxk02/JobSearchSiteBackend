using Core.Domains._Shared.Repositories;
using Core.Domains._Shared.UnitOfWork;
using Core.Domains.Companies.UseCases.CreateCompany;
using Core.Domains.CompanyPermissions;
using Core.Domains.FolderPermissions;
using Core.Domains.Folders;
using Core.Services.Auth.Authentication;
using Shared.Result;

namespace Core.Domains.Companies;

public class CompanyService(IRepository<Company> companyRepository,
    IUnitOfWork unitOfWork,
    ICompanyPermissionRepository companyPermissionRepository,
    IRepository<Folder> folderRepository,
    IFolderPermissionRepository folderPermissionRepository,
    ICurrentAccountService currentAccountService) : ICompanyService
{
    public async Task<Result<long>> CreateCompanyAsync(CreateCompanyRequest request,
        CancellationToken cancellationToken = default)
    {
        //creating company and checking result
        var companyCreationResult =
            Company.Create(request.Name, request.Description, request.IsPublic, request.CountryId);

        if (companyCreationResult.IsFailure)
            return Result<long>.WithMetadataFrom(companyCreationResult);

        // starting transaction
        await unitOfWork.BeginAsync(cancellationToken);
        
        //adding company
        var createdCompany = await companyRepository.AddAsync(companyCreationResult.Value, cancellationToken);
        //adding full permission set to user
        await companyPermissionRepository.UpdatePermissionsForUserAsync(currentAccountService.GetIdOrThrow(), 
            createdCompany.Id, CompanyPermission.AllIds, cancellationToken);
        
        //creating folder and checking result
        var rootFolderCreationResult = Folder.Create(createdCompany.Id, null, null, null);
        
        if (rootFolderCreationResult.IsFailure)
            return Result<long>.Error();
        
        //adding root folder to company
        var createdFolder = await folderRepository.AddAsync(rootFolderCreationResult.Value, cancellationToken);
        //adding full permission set to user
        await folderPermissionRepository.UpdatePermissionsForUserAsync(currentAccountService.GetIdOrThrow(),
            createdFolder.Id, FolderPermission.AllIds, cancellationToken);
        
        // committing transaction
        await unitOfWork.CommitAsync(cancellationToken);

        return createdCompany.Id;
    }

    public async Task<Result> DeleteCompanyAsync(long companyId, CancellationToken cancellationToken = default)
    {
        var company = await companyRepository.GetByIdAsync(companyId, cancellationToken);

        if (company is null)
            return Result.NotFound();
        
        await companyRepository.RemoveAsync(company, cancellationToken);
        
        return Result.Success();
    }
}
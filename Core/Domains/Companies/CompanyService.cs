using Core.Domains._Shared.Repositories;
using Core.Domains._Shared.UnitOfWork;
using Core.Domains.Companies.UseCases.CreateCompany;
using Core.Domains.Companies.UseCases.GetCompanyById;
using Core.Domains.Companies.UseCases.UpdateCompany;
using Core.Domains.CompanyPermissions;
using Core.Domains.FolderPermissions;
using Core.Domains.Folders;
using Core.Services.Auth.Authentication;
using Shared.Result;

namespace Core.Domains.Companies;

public class CompanyService(
    ICompanyRepository companyRepository,
    IUnitOfWork unitOfWork,
    ICompanyPermissionRepository companyPermissionRepository,
    IRepository<Folder> folderRepository,
    IFolderPermissionRepository folderPermissionRepository,
    ICurrentAccountService currentAccountService) : ICompanyService
{
    public async Task<Result<long>> CreateCompanyAsync(CreateCompanyRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

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
        await companyPermissionRepository.UpdatePermissionIdsForUserAsync(currentUserId, createdCompany.Id,
            CompanyPermission.AllIds, cancellationToken);

        //creating folder and checking result
        var rootFolderCreationResult = Folder.Create(createdCompany.Id, null, null, null);

        if (rootFolderCreationResult.IsFailure)
            return Result<long>.Error();

        //adding root folder to company
        var createdFolder = await folderRepository.AddAsync(rootFolderCreationResult.Value, cancellationToken);
        //adding full permission set to user
        await folderPermissionRepository.UpdatePermissionsForUserAsync(currentUserId, createdFolder.Id,
            FolderPermission.AllIds, cancellationToken);

        // committing transaction
        await unitOfWork.CommitAsync(cancellationToken);

        return createdCompany.Id;
    }

    public async Task<Result> DeleteCompanyAsync(long companyId, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var companyWithPermissionIdsDto = await companyRepository.GetCompanyWithPermissionIdsForUser(currentUserId,
            companyId, cancellationToken);

        if (companyWithPermissionIdsDto.Company is null)
            return Result.NotFound("Requested company does not exist.");
        
        if (!companyWithPermissionIdsDto.PermissionIds.Contains(CompanyPermission.HasFullAccess.Id))
        {
            return Result.Forbidden("Insufficient permissions for requested company.");
        }

        await companyRepository.RemoveAsync(companyWithPermissionIdsDto.Company, cancellationToken);

        return Result.Success();
    }
    
    public async Task<Result> UpdateCompanyAsync(UpdateCompanyRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var companyWithPermissionIdsDto = await companyRepository.GetCompanyWithPermissionIdsForUser(currentUserId,
            request.CompanyId, cancellationToken);

        if (companyWithPermissionIdsDto.Company is null)
            return Result.NotFound("Requested company does not exist.");
        
        if (!companyWithPermissionIdsDto.PermissionIds.Contains(CompanyPermission.CanEditProfile.Id))
        {
            return Result.Forbidden("Insufficient permissions for requested company.");
        }

        var updatedCompanyResult = Company.Create(
            request.Name ?? companyWithPermissionIdsDto.Company.Name,
            request.Description ?? companyWithPermissionIdsDto.Company.Description,
            request.IsPublic ?? companyWithPermissionIdsDto.Company.IsPublic,
            companyWithPermissionIdsDto.Company.CountryId,
            companyWithPermissionIdsDto.Company.Id
            );
        
        if (updatedCompanyResult.IsFailure)
            return Result.WithMetadataFrom(updatedCompanyResult);

        await companyRepository.UpdateAsync(updatedCompanyResult.Value, cancellationToken);

        return Result.Success();
    }

    public async Task<Result<GetCompanyByIdResponse>> GetCompanyByIdAsync(long companyId,
        CancellationToken cancellationToken = default)
    {
        var company = await companyRepository.GetByIdAsync(companyId, cancellationToken);
        
        if (company is null)
            return Result<GetCompanyByIdResponse>.NotFound("Requested company does not exist.");

        if (!company.IsPublic)
        {
            var currentUserId = currentAccountService.GetId();

            if (currentUserId is null)
                return Result<GetCompanyByIdResponse>.Forbidden("Requested company profile is private.");
            
            var permissionIds = await companyPermissionRepository.GetPermissionIdsForUserAsync(
                currentUserId.Value, companyId, cancellationToken);
            
            if (!permissionIds.Contains(CompanyPermission.CanEditProfile.Id))
                return Result<GetCompanyByIdResponse>.Forbidden("Requested company profile is private.");
        }

        return new GetCompanyByIdResponse(company.Name, company.Description, company.CountryId);
    }
}
using Core.Domains._Shared.Repositories;
using Core.Domains._Shared.UnitOfWork;
using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.CompanyPermissions;
using Core.Domains.JobFolderPermissions;
using Core.Domains.JobFolders;
using Core.Services.Auth.Authentication;
using Shared.Result;

namespace Core.Domains.Companies.UseCases.CreateCompany;

public class CreateCompanyHandler(
    ICurrentAccountService currentAccountService,
    ICompanyRepository companyRepository,
    IRepository<JobFolder> folderRepository,
    ICompanyPermissionRepository companyPermissionRepository,
    IJobFolderPermissionRepository jobFolderPermissionRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateCompanyRequest, Result<CreateCompanyResponse>>
{
    public async Task<Result<CreateCompanyResponse>> Handle(CreateCompanyRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        //creating company and checking result
        var companyCreationResult =
            Company.Create(request.Name, request.Description, request.IsPublic, request.CountryId);

        if (companyCreationResult.IsFailure)
            return Result<CreateCompanyResponse>.WithMetadataFrom(companyCreationResult);

        // starting transaction
        await unitOfWork.BeginAsync(cancellationToken);

        //adding company
        var createdCompany = await companyRepository.AddAsync(companyCreationResult.Value, cancellationToken);
        //adding full permission set to user
        await companyPermissionRepository.UpdatePermissionIdsForUserAsync(currentUserId, createdCompany.Id,
            CompanyPermission.AllIds, cancellationToken);

        //creating folder and checking result
        var rootFolderCreationResult = JobFolder.Create(createdCompany.Id, null, null, null);

        if (rootFolderCreationResult.IsFailure)
            return Result<CreateCompanyResponse>.Error();

        //adding root folder to company
        var createdFolder = await folderRepository.AddAsync(rootFolderCreationResult.Value, cancellationToken);
        //adding full permission set to user
        await jobFolderPermissionRepository.UpdatePermissionIdsForUserAsync(currentUserId, createdFolder.Id,
            JobFolderPermission.AllIds, cancellationToken);

        // committing transaction
        await unitOfWork.CommitAsync(cancellationToken);

        return new CreateCompanyResponse(createdCompany.Id);
    }
}
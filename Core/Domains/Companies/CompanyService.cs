using Core.Domains._Shared.Repositories;
using Core.Domains._Shared.UnitOfWork;
using Core.Domains.Companies.UseCases.AddCompany;
using Core.Domains.CompanyPermissions.UserCompanyCompanyPermissions;
using Core.Services.Auth.Authentication;
using Shared.Result;

namespace Core.Domains.Companies;

public class CompanyService(IRepository<Company> companyRepository,
    IUnitOfWork unitOfWork,
    IRepository<UserCompanyCompanyPermission> uccpRepository,
    ICurrentAccountService currentAccountService) : ICompanyService
{
    public async Task<Result<long>> AddCompanyAsync(AddCompanyRequest request,
        CancellationToken cancellationToken = default)
    {
        var companyToAddCreationResult =
            Company.Create(request.Name, request.Description, request.IsPublic, request.CountryId);

        if (companyToAddCreationResult.Value is null)
            return Result<long>.WithMetadataFrom(companyToAddCreationResult);

        await unitOfWork.BeginAsync(cancellationToken);
        
        var createdCompany = await companyRepository.AddAsync(companyToAddCreationResult.Value, cancellationToken);
        
        // var newPermission
        
        // await uccpRepository.AddAsync(new UserCompanyCompanyPermission(currentAccountService.GetIdOrThrow(), createdCompany.Id), cancellationToken);
        
        await unitOfWork.CommitAsync(cancellationToken);

        return createdCompany.Id;
    }
}
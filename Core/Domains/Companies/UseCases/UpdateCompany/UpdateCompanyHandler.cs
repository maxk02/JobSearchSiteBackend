using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.CompanyPermissions;
using Core.Services.Auth.Authentication;
using Shared.Result;

namespace Core.Domains.Companies.UseCases.UpdateCompany;

public class UpdateCompanyHandler(ICurrentAccountService currentAccountService,
    ICompanyRepository companyRepository) : IRequestHandler<UpdateCompanyRequest, Result>
{
    public async Task<Result> Handle(UpdateCompanyRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var companyWithPermissionIdsDto = await companyRepository.GetCompanyWithPermissionIdsForUser(currentUserId,
            request.CompanyId, cancellationToken);

        if (companyWithPermissionIdsDto.Company is null)
            return Result.NotFound("Requested company does not exist.");
        
        if (!companyWithPermissionIdsDto.PermissionIds.Contains(CompanyPermission.CanEditProfile.Id))
        {
            return Result.Forbidden("Insufficient permissions for company update.");
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
}
using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.CompanyPermissions;
using Core.Services.Auth.Authentication;
using Shared.Result;

namespace Core.Domains.Companies.UseCases.GetCompanyById;

public class GetCompanyByIdHandler(
    ICurrentAccountService currentAccountService,
    ICompanyRepository companyRepository,
    ICompanyPermissionRepository companyPermissionRepository)
    : IRequestHandler<GetCompanyByIdRequest, Result<GetCompanyByIdResponse>>
{
    public async Task<Result<GetCompanyByIdResponse>> Handle(GetCompanyByIdRequest request,
        CancellationToken cancellationToken = default)
    {
        var company = await companyRepository.GetByIdAsync(request.Id, cancellationToken);

        if (company is null)
            return Result<GetCompanyByIdResponse>.NotFound("Requested company does not exist.");

        if (!company.IsPublic)
        {
            var currentUserId = currentAccountService.GetId();

            if (currentUserId is null)
                return Result<GetCompanyByIdResponse>.Forbidden("Requested company profile is private.");

            var permissionIds = await companyPermissionRepository.GetPermissionIdsForUserAsync(
                currentUserId.Value, request.Id, cancellationToken);

            if (!permissionIds.Contains(CompanyPermission.CanEditProfile.Id))
                return Result<GetCompanyByIdResponse>.Forbidden("Requested company profile is private.");
        }

        return new GetCompanyByIdResponse(company.Name, company.Description, company.CountryId);
    }
}
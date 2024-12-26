using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.CompanyPermissions;
using Core.Services.Auth.Authentication;
using Shared.Result;

namespace Core.Domains.Companies.UseCases.DeleteCompany;

public class DeleteCompanyHandler(ICurrentAccountService currentAccountService,
    ICompanyRepository companyRepository) : IRequestHandler<DeleteCompanyRequest, Result>
{
    public async Task<Result> Handle(DeleteCompanyRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var companyWithPermissionIdsDto = await companyRepository.GetCompanyWithPermissionIdsForUser(currentUserId,
            request.Id, cancellationToken);

        if (companyWithPermissionIdsDto.Company is null)
            return Result.NotFound("Requested company does not exist.");

        if (!companyWithPermissionIdsDto.PermissionIds.Contains(CompanyPermission.IsOwner.Id))
        {
            return Result.Forbidden("Insufficient permissions for company deletion.");
        }

        await companyRepository.RemoveAsync(companyWithPermissionIdsDto.Company, cancellationToken);

        return Result.Success();
    }
}
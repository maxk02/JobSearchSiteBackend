using Core.Domains.CompanyPermissions.UseCases.GetCompanyPermissionIdsForUser;
using Core.Domains.CompanyPermissions.UseCases.UpdateCompanyPermissionIdsForUser;
using Shared.Result;

namespace Core.Domains.CompanyPermissions;

public interface ICompanyPermissionService
{
    public Task<Result<ICollection<long>>> GetCompanyPermissionIdsForUserAsync(GetCompanyPermissionIdsForUserRequest request,
        CancellationToken cancellationToken = default);
    public Task<Result> UpdateCompanyPermissionIdsForUserAsync(UpdateCompanyPermissionIdsForUserRequest request,
        CancellationToken cancellationToken = default);
}
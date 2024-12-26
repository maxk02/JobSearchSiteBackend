using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.CompanyPermissions.UseCases.UpdateCompanyPermissionIdsForUser;

public record UpdateCompanyPermissionIdsForUserRequest(long UserId, long CompanyId,
    ICollection<long> CompanyPermissionIds) : IRequest<Result>;
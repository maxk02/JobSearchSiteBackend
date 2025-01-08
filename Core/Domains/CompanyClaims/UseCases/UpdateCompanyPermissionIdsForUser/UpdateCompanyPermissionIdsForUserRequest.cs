using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.CompanyClaims.UseCases.UpdateCompanyPermissionIdsForUser;

public record UpdateCompanyPermissionIdsForUserRequest(long UserId, long CompanyId,
    ICollection<long> CompanyPermissionIds) : IRequest<Result>;
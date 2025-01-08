using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.CompanyClaims.UseCases.GetCompanyPermissionIdsForUser;

public record GetCompanyPermissionIdsForUserRequest(long UserId, long CompanyId) : IRequest<Result<ICollection<long>>>;
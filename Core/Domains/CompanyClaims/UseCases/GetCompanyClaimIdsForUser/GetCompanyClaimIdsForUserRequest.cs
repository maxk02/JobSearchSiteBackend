using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.CompanyClaims.UseCases.GetCompanyClaimIdsForUser;

public record GetCompanyClaimIdsForUserRequest(long UserId, long CompanyId) : IRequest<Result<ICollection<long>>>;
using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.CompanyClaims.UseCases.UpdateCompanyClaimIdsForUser;

public record UpdateCompanyClaimIdsForUserRequest(long UserId, long CompanyId,
    ICollection<long> CompanyClaimIds) : IRequest<Result>;
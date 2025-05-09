using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.CompanyClaims.UseCases.UpdateCompanyClaimIdsForUser;

public record UpdateCompanyClaimIdsForUserRequest(long UserId, long CompanyId,
    ICollection<long> CompanyClaimIds) : IRequest<Result>;
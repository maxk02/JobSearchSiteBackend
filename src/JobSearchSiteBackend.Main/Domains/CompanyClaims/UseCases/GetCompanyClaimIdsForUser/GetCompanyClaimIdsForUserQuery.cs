using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Main.Domains.CompanyClaims.UseCases.GetCompanyClaimIdsForUser;

public record GetCompanyClaimIdsForUserQuery(long UserId, long CompanyId)
    : IRequest<Result<GetCompanyClaimIdsForUserResult>>;
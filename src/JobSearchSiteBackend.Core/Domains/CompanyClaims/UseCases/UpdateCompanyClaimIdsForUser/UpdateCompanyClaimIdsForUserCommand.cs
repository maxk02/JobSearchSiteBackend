using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.CompanyClaims.UseCases.UpdateCompanyClaimIdsForUser;

public record UpdateCompanyClaimIdsForUserCommand(long UserId, long CompanyId,
    ICollection<long> CompanyClaimIds, string? PasswordForConfirmation) : IRequest<Result>;
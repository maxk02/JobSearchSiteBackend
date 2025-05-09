using Ardalis.Result;
using Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.API.Controllers.CompanyClaims;

public record UpdateCompanyClaimIdsForUserRequestDto(ICollection<long> CompanyClaimIds) : IRequest<Result>;
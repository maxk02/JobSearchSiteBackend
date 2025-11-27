namespace JobSearchSiteBackend.API.Controllers.CompanyClaims.Dtos;

public record GetCompanyClaimIdsForUserResponse(ICollection<long> ClaimIds);
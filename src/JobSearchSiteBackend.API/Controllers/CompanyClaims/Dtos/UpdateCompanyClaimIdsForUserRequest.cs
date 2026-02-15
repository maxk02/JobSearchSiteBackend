using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.API.Controllers.CompanyClaims.Dtos;

public record UpdateCompanyClaimIdsForUserRequest(ICollection<long> CompanyClaimIds, string? PasswordForConfirmation);
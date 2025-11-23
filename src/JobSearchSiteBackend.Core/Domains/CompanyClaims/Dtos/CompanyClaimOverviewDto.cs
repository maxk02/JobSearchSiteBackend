namespace JobSearchSiteBackend.Core.Domains.CompanyClaims.Dtos;

public record CompanyClaimOverviewDto(
    long UserCompanyClaimId,
    long UserId,
    string UserFirstName,
    string UserLastName,
    string UserEmail,
    long ClaimId
    );
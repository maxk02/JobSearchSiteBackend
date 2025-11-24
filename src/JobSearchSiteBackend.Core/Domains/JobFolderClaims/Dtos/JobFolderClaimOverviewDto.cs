namespace JobSearchSiteBackend.Core.Domains.JobFolderClaims.Dtos;

public record JobFolderClaimOverviewDto(
    long UserCompanyClaimId,
    long UserId,
    string UserFirstName,
    string UserLastName,
    string UserEmail,
    long ClaimId,
    bool IsInherited,
    JobFolderClaimSourceFolderDto? InheritedFrom
);
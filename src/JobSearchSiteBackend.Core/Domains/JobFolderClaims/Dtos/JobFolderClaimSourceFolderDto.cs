namespace JobSearchSiteBackend.Core.Domains.JobFolderClaims.Dtos;

public record JobFolderClaimSourceFolderDto(
    long SourceFolderId,
    string? SourceFolderName
);
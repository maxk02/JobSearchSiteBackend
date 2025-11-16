namespace JobSearchSiteBackend.Core.Domains.JobFolders.Dtos;

public record JobFolderDetailedDto(
    long Id,
    string? Name,
    string? Description,
    long? RootFolderId,
    long? ParentFolderId,
    long CompanyId,
    string CompanyName,
    string? CompanyLogoLink,
    ICollection<long> ClaimIds,
    ICollection<JobFolderMinimalDto> Children
    );
namespace JobSearchSiteBackend.Core.Domains.JobFolders.Dtos;

public record JobFolderMinimalDto(long Id, string? Name, ICollection<long> ClaimIds);
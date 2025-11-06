namespace JobSearchSiteBackend.Core.Domains.Companies.Dtos;

public record CompanyJobFolderListItemDto(long Id, string Name, ICollection<long> ClaimIds);
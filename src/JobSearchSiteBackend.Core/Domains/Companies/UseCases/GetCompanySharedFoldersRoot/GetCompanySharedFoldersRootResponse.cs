using JobSearchSiteBackend.Core.Domains.JobFolders.Dtos;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanySharedFoldersRoot;

public record GetCompanySharedFoldersRootResponse(ICollection<JobFolderMinimalDto> Folders);
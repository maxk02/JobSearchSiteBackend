using JobSearchSiteBackend.Core.Domains.JobFolders.Dtos;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanySharedFoldersRoot;

public record GetCompanySharedFoldersRootResult(ICollection<JobFolderMinimalDto> Folders);
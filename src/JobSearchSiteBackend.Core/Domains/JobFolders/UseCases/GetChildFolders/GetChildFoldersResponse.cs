using JobSearchSiteBackend.Core.Domains.JobFolders.Dtos;
using JobSearchSiteBackend.Core.Domains.Jobs.Dtos;

namespace JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.GetChildFolders;

public record GetChildFoldersResponse(ICollection<JobFolderMinimalDto> Folders);
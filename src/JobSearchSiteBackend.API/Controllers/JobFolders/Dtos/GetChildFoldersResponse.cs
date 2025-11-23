using JobSearchSiteBackend.Core.Domains.JobFolders.Dtos;

namespace JobSearchSiteBackend.API.Controllers.JobFolders.Dtos;

public record GetChildFoldersResponse(ICollection<JobFolderMinimalDto> Folders);
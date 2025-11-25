using JobSearchSiteBackend.Core.Domains.JobFolders.Dtos;

namespace JobSearchSiteBackend.API.Controllers.Companies.Dtos;

public record GetCompanySharedFoldersRootResponse(ICollection<JobFolderMinimalDto> Folders); //todo
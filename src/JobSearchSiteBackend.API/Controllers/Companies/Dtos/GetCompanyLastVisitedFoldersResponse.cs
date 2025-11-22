using JobSearchSiteBackend.Core.Domains.Companies.Dtos;

namespace JobSearchSiteBackend.API.Controllers.Companies.Dtos;

public record GetCompanyLastVisitedFoldersResponse(ICollection<CompanyJobFolderListItemDto> JobFolders);
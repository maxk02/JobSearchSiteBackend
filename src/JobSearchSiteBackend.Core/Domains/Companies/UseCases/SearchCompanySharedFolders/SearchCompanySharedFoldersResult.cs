using JobSearchSiteBackend.Core.Domains.Companies.Dtos;
using JobSearchSiteBackend.Core.Domains.JobFolders.Dtos;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.SearchCompanySharedFolders;

public record SearchCompanySharedFoldersResult(ICollection<CompanyJobFolderListItemDto> Folders);
using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains.JobFolders.Dtos;
using JobSearchSiteBackend.Core.Domains.Jobs.Dtos;

namespace JobSearchSiteBackend.API.Controllers.JobFolders.Dtos;

public record GetFolderJobsResponse(ICollection<JobManagementCardDto> Jobs, //todo check dto
    PaginationResponse PaginationResponse);
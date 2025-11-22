using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains.Jobs.Dtos;

namespace JobSearchSiteBackend.API.Controllers.Companies.Dtos;

public record GetCompanyJobsResponse(
    ICollection<JobCardDto> JobCards,
    PaginationResponse PaginationResponse);
using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains.Jobs.Dtos;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyJobs;

public record GetCompanyJobsResult(
    ICollection<JobCardDto> JobCards,
    PaginationResponse PaginationResponse);
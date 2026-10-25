using JobSearchSiteBackend.Main.Domains._Shared.Pagination;
using JobSearchSiteBackend.Main.Domains.Jobs.Dtos;

namespace JobSearchSiteBackend.Main.Domains.Companies.UseCases.GetCompanyJobs;

public record GetCompanyJobsResult(
    ICollection<JobCardDto> JobCards,
    PaginationResponse PaginationResponse);
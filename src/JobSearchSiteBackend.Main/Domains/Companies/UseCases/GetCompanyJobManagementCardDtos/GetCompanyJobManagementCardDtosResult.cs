using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains.Jobs.Dtos;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyJobManagementCardDtos;

public record GetCompanyJobManagementCardDtosResult(
    ICollection<JobManagementCardDto> JobManagementCardDtos,
    PaginationResponse PaginationResponse);
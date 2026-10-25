using JobSearchSiteBackend.Main.Domains._Shared.Pagination;
using JobSearchSiteBackend.Main.Domains.Jobs.Dtos;

namespace JobSearchSiteBackend.Main.Domains.Companies.UseCases.GetCompanyJobManagementCardDtos;

public record GetCompanyJobManagementCardDtosResult(
    ICollection<JobManagementCardDto> JobManagementCardDtos,
    PaginationResponse PaginationResponse);
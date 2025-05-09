using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains.Companies.Dtos;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanies;

public record GetCompaniesResponse(
    ICollection<CompanyDto> CompanyInfoDtos,
    PaginationResponse PaginationResponse);
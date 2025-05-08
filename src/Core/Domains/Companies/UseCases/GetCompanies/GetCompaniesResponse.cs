using Core.Domains._Shared.Pagination;
using Core.Domains.Companies.Dtos;

namespace Core.Domains.Companies.UseCases.GetCompanies;

public record GetCompaniesResponse(
    ICollection<CompanyDto> CompanyInfoDtos,
    PaginationResponse PaginationResponse);
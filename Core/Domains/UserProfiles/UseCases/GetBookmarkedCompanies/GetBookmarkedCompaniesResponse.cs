using Core.Domains._Shared.Pagination;
using Core.Domains.Companies.Dtos;

namespace Core.Domains.UserProfiles.UseCases.GetBookmarkedCompanies;

public record GetBookmarkedCompaniesResponse(
    ICollection<CompanyInfoDto> CompanyInfoDtos,
    PaginationResponse PaginationResponse);
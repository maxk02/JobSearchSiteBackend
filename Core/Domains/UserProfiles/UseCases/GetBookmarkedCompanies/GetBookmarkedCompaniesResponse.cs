using Core.Domains._Shared.Pagination;
using Core.Domains.Companies.Dtos;

namespace Core.Domains.UserProfiles.UseCases.GetBookmarkedCompanies;

public record GetBookmarkedCompaniesResponse(
    ICollection<CompanyInfocardDto> CompanyInfocardDtos,
    PaginationResponse PaginationResponse);
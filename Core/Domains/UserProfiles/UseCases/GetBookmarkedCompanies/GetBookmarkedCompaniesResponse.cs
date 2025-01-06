using Core.Domains._Shared.Pagination;
using Core.Domains.UserProfiles.Dtos;

namespace Core.Domains.UserProfiles.UseCases.GetBookmarkedCompanies;

public record GetBookmarkedCompaniesResponse(
    ICollection<CompanyShortDto> CompanyShortDto,
    PaginationResponse PaginationResponse);
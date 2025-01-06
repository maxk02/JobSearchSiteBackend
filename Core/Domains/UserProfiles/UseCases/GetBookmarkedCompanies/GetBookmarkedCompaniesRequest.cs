using Core.Domains._Shared.Pagination;
using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.UserProfiles.UseCases.GetBookmarkedCompanies;

public record GetBookmarkedCompaniesRequest(long UserId, PaginationSpec PaginationSpec) 
    : IRequest<Result<GetBookmarkedCompaniesResponse>>;
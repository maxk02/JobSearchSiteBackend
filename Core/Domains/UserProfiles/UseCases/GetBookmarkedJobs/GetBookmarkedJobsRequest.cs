using Core.Domains._Shared.Pagination;
using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.UserProfiles.UseCases.GetBookmarkedJobs;

public record GetBookmarkedJobsRequest(long UserId, PaginationSpec PaginationSpec) 
    : IRequest<Result<GetBookmarkedJobsResponse>>;
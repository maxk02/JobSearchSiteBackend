using Core.Domains._Shared.Pagination;
using Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace Core.Domains.UserProfiles.UseCases.GetBookmarkedJobs;

public record GetBookmarkedJobsRequest(PaginationSpec PaginationSpec) 
    : IRequest<Result<GetBookmarkedJobsResponse>>;
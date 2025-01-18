using Core.Domains._Shared.Pagination;
using Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace Core.Domains.UserProfiles.UseCases.GetJobApplicationsForUser;

public record GetJobApplicationsForUserRequest(long UserId, PaginationSpec PaginationSpec) 
    : IRequest<Result<GetJobApplicationsForUserResponse>>;
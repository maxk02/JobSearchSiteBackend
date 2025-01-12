using Core.Domains._Shared.Pagination;
using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.UserProfiles.UseCases.GetJobApplicationsForUser;

public record GetJobApplicationsForUserRequest(long UserId, PaginationSpec PaginationSpec) 
    : IRequest<Result<GetJobApplicationsForUserResponse>>;
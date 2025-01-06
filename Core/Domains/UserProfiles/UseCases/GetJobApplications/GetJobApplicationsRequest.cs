using Core.Domains._Shared.Pagination;
using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.UserProfiles.UseCases.GetJobApplications;

public record GetJobApplicationsRequest(long UserId, PaginationSpec PaginationSpec) 
    : IRequest<Result<GetJobApplicationsResponse>>;
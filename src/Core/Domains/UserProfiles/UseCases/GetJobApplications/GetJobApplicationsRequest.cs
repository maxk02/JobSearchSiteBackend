using Ardalis.Result;
using Core.Domains._Shared.Pagination;
using Core.Domains._Shared.UseCaseStructure;

namespace Core.Domains.UserProfiles.UseCases.GetJobApplications;

public record GetJobApplicationsRequest(PaginationSpec PaginationSpec) 
    : IRequest<Result<GetJobApplicationsResponse>>;
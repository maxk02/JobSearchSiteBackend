using Core.Domains._Shared.Pagination;
using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.JobApplications.UseCases.GetApplicationsForJobId;

public record GetApplicationsForJobIdRequest(long JobId, string Query, PaginationSpec PaginationSpec)
    : IRequest<Result<GetApplicationsForJobIdResponse>>;
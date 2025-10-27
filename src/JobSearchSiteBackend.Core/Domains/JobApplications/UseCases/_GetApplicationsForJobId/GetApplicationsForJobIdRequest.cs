using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.JobApplications.UseCases._GetApplicationsForJobId;

public record GetApplicationsForJobIdRequest(long JobId, string Query, PaginationSpec PaginationSpec)
    : IRequest<Result<GetApplicationsForJobIdResponse>>;
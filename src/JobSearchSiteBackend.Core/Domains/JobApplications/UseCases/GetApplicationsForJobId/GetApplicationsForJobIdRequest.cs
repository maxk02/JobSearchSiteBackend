using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.GetApplicationsForJobId;

public record GetApplicationsForJobIdRequest(long JobId, string Query, PaginationSpec PaginationSpec)
    : IRequest<Result<GetApplicationsForJobIdResponse>>;
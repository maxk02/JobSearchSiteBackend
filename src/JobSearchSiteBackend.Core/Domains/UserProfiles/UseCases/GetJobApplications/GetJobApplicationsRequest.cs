using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.GetJobApplications;

public record GetJobApplicationsRequest(PaginationSpec PaginationSpec) 
    : IRequest<Result<GetJobApplicationsResponse>>;
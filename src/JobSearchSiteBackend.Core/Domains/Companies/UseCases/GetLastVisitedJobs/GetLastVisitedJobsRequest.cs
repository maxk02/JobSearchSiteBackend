using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetLastVisitedJobs;

public record GetLastVisitedJobsRequest(): IRequest<GetLastVisitedJobsResponse>;
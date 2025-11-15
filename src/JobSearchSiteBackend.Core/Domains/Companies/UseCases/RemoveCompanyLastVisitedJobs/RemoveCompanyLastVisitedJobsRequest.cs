using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.RemoveCompanyLastVisitedJobs;

public record RemoveCompanyLastVisitedJobsRequest(long CompanyId, long? SingleJobId = null): IRequest<Result>;
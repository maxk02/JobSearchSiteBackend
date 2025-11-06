using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyLastVisitedJobs;

public record GetCompanyLastVisitedJobsRequest(long CompanyId): IRequest<Result<GetCompanyLastVisitedJobsResponse>>;
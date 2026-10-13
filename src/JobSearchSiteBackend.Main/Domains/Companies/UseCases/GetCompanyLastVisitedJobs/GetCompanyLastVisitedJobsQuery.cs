using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyLastVisitedJobs;

public record GetCompanyLastVisitedJobsQuery(long CompanyId): IRequest<Result<GetCompanyLastVisitedJobsResult>>;
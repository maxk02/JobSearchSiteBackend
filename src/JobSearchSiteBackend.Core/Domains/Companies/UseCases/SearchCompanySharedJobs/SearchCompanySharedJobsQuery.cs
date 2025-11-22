using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.SearchCompanySharedJobs;

public record SearchCompanySharedJobsQuery(long CompanyId, string Query) : IRequest<Result<SearchCompanySharedJobsResult>>;
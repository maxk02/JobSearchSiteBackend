using Ardalis.Result;
using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Main.Domains.Companies.UseCases.SearchCompanySharedJobs;

public record SearchCompanySharedJobsQuery(long CompanyId, string Query) : IRequest<Result<SearchCompanySharedJobsResult>>;
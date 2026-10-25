using Ardalis.Result;
using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Main.Domains.Companies.UseCases.RemoveCompanyLastVisitedJobs;

public record RemoveCompanyLastVisitedJobsCommand(long CompanyId, long? SingleJobId = null): IRequest<Result>;
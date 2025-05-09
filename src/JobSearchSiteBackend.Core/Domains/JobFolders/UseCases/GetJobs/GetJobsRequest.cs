using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.GetJobs;

public record GetJobsRequest(long Id) : IRequest<Result<GetJobsResponse>>;
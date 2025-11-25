using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.GetJobs;

public record GetFolderJobsQuery(long Id) : IRequest<Result<GetFolderJobsResult>>;
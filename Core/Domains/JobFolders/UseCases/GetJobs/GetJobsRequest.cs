using Ardalis.Result;
using Core.Domains._Shared.UseCaseStructure;

namespace Core.Domains.JobFolders.UseCases.GetJobs;

public record GetJobsRequest(long Id) : IRequest<Result<GetJobsResponse>>;
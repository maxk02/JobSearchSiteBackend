using Ardalis.Result;
using Core.Domains._Shared.UseCaseStructure;

namespace Core.Domains.JobFolders.UseCases.GetJobs;

public record GetJobsRequest(long JobFolderId) : IRequest<Result<GetJobsResponse>>;
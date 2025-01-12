using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.JobFolders.UseCases.GetChildJobsAndFolders;

public record GetChildJobsAndFoldersRequest(long JobFolderId) : IRequest<Result<GetChildJobsAndFoldersResponse>>;
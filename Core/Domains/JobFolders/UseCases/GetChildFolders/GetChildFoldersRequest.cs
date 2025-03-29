using Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace Core.Domains.JobFolders.UseCases.GetChildFolders;

public record GetChildFoldersRequest(long JobFolderId) : IRequest<Result<GetChildFoldersResponse>>;
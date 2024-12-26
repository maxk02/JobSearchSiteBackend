using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.FolderPermissions.UseCases.GetFolderPermissionIdsForUser;

public record GetFolderPermissionIdsForUserRequest(long UserId, long FolderId) : IRequest<Result<ICollection<long>>>;
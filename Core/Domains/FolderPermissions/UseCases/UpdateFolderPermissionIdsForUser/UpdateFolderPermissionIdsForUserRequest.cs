using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.FolderPermissions.UseCases.UpdateFolderPermissionIdsForUser;

public record UpdateFolderPermissionIdsForUserRequest(long UserId, long FolderId, ICollection<long> FolderPermissionIds)
    : IRequest<Result>;
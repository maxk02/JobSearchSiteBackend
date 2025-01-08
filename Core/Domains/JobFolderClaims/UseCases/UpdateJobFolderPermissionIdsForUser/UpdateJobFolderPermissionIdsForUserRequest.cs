using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.JobFolderClaims.UseCases.UpdateJobFolderPermissionIdsForUser;

public record UpdateJobFolderPermissionIdsForUserRequest(long UserId, long FolderId, ICollection<long> FolderPermissionIds)
    : IRequest<Result>;
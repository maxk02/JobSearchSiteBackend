using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.JobFolderPermissions.UseCases.GetJobFolderPermissionIdsForUser;

public record GetJobFolderPermissionIdsForUserRequest(long UserId, long FolderId) : IRequest<Result<ICollection<long>>>;
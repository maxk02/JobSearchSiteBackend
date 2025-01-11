using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.JobFolderClaims.UseCases.UpdateJobFolderClaimIdsForUser;

public record UpdateJobFolderClaimIdsForUserRequest(long UserId, long FolderId,
    ICollection<long> JobFolderClaimIds) : IRequest<Result>;
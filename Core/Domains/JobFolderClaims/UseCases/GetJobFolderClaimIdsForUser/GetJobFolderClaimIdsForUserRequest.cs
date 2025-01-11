using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.JobFolderClaims.UseCases.GetJobFolderClaimIdsForUser;

public record GetJobFolderClaimIdsForUserRequest(long UserId, long FolderId) : IRequest<Result<ICollection<long>>>;
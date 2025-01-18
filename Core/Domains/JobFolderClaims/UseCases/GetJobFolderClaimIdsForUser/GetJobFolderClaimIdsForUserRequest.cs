using Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace Core.Domains.JobFolderClaims.UseCases.GetJobFolderClaimIdsForUser;

public record GetJobFolderClaimIdsForUserRequest(long UserId, long FolderId) : IRequest<Result<ICollection<long>>>;
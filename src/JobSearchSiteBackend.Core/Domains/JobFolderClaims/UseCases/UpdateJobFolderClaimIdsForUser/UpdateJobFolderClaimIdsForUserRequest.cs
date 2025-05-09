using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.JobFolderClaims.UseCases.UpdateJobFolderClaimIdsForUser;

public record UpdateJobFolderClaimIdsForUserRequest(long UserId, long FolderId,
    ICollection<long> JobFolderClaimIds) : IRequest<Result>;
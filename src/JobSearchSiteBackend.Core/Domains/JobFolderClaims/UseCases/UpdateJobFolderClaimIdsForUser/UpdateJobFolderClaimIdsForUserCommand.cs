using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.JobFolderClaims.UseCases.UpdateJobFolderClaimIdsForUser;

public record UpdateJobFolderClaimIdsForUserCommand(long UserId, long JobFolderId,
    ICollection<long> JobFolderClaimIds) : IRequest<Result>;
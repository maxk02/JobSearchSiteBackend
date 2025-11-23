using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.API.Controllers.JobFolderClaims.Dtos;

public record UpdateJobFolderClaimIdsForUserRequest(long UserId, long FolderId,
    ICollection<long> JobFolderClaimIds) : IRequest<Result>;
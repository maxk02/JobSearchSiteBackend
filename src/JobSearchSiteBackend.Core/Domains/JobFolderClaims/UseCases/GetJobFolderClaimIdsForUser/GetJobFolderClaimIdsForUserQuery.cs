using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.JobFolderClaims.UseCases.GetJobFolderClaimIdsForUser;

public record GetJobFolderClaimIdsForUserQuery(long UserId, long FolderId) : IRequest<Result<ICollection<long>>>;
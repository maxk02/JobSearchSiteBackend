using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.API.Controllers.JobFolderClaims;

public record UpdateJobFolderClaimIdsForUserRequestDto(ICollection<long> JobFolderClaimIds) : IRequest<Result>;
using Ardalis.Result;
using Core.Domains._Shared.UseCaseStructure;

namespace API.Controllers.JobFolderClaims;

public record UpdateJobFolderClaimIdsForUserRequestDto(ICollection<long> JobFolderClaimIds) : IRequest<Result>;
using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.API.Controllers.JobFolderClaims.Dtos;

public record GetJobFolderClaimsOverviewRequest(
    string UserQuery,
    ICollection<long> JobFolderClaimIds,
    int Page,
    int Size) : IRequest<Result<GetJobFolderClaimsOverviewResponse>>;
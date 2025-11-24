using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.JobFolderClaims.UseCases.GetJobFolderClaimsOverview;

public record GetJobFolderClaimsOverviewQuery(
    long JobFolderId,
    string UserQuery,
    ICollection<long> JobFolderClaimIds,
    int Page,
    int Size) : IRequest<Result<GetJobFolderClaimsOverviewResult>>;
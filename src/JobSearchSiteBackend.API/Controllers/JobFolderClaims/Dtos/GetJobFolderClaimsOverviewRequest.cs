namespace JobSearchSiteBackend.API.Controllers.JobFolderClaims.Dtos;

public record GetJobFolderClaimsOverviewRequest(
    string? UserQuery,
    ICollection<long>? JobFolderClaimIds,
    int Page,
    int Size);
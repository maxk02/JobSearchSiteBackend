namespace JobSearchSiteBackend.API.Controllers.JobFolders.Dtos;

public record GetFolderJobsRequest(
    string? Query,
    int Page,
    int Size); //todo frontend pagination
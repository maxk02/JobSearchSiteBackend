namespace JobSearchSiteBackend.API.Controllers.JobFolders.Dtos;

public record AddJobFolderRequest(long CompanyId, long ParentId,
    string? Name, string? Description);
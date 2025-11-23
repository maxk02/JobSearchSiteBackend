using JobSearchSiteBackend.Core.Domains.JobFolders.Dtos;

namespace JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.GetJobFolder;

public record GetJobFolderResult(JobFolderDetailedDto Folder);
using JobSearchSiteBackend.Core.Domains.Jobs.Dtos;

namespace JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.GetFolderJobs;

public record GetFolderJobsResult(ICollection<JobCardDto> Jobs);
using JobSearchSiteBackend.Core.Domains.Jobs.Dtos;

namespace JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.GetJobs;

public record GetFolderJobsResult(ICollection<JobCardDto> Jobs);
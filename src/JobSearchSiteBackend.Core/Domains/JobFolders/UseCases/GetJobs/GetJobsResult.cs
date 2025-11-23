using JobSearchSiteBackend.Core.Domains.Jobs.Dtos;

namespace JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.GetJobs;

public record GetJobsResult(ICollection<JobCardDto> Jobs);
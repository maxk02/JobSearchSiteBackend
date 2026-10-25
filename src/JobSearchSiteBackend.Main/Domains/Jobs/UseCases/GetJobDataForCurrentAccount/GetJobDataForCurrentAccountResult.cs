using JobSearchSiteBackend.Main.Domains.JobApplications.Dtos;
using JobSearchSiteBackend.Main.Domains.Jobs.Dtos;

namespace JobSearchSiteBackend.Main.Domains.Jobs.UseCases.GetJob;

public record GetJobDataForCurrentAccountResult(JobApplicationOnJobPageDto? JobApplicationOnJobPageDto, bool IsBookmarked);
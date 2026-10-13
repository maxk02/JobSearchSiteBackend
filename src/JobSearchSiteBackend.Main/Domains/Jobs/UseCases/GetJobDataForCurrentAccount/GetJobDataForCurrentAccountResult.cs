using JobSearchSiteBackend.Core.Domains.JobApplications.Dtos;
using JobSearchSiteBackend.Core.Domains.Jobs.Dtos;

namespace JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetJob;

public record GetJobDataForCurrentAccountResult(JobApplicationOnJobPageDto? JobApplicationOnJobPageDto, bool IsBookmarked);
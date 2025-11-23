using JobSearchSiteBackend.Core.Domains.Jobs.Dtos;

namespace JobSearchSiteBackend.API.Controllers.JobFolders.Dtos;

public record GetJobsResponse(ICollection<JobCardDto> Jobs);
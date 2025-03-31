using Core.Domains.Jobs.Dtos;

namespace Core.Domains.JobFolders.UseCases.GetJobs;

public record GetJobsResponse(ICollection<JobCardDto> Jobs);
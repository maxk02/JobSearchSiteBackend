using JobSearchSiteBackend.Core.Domains.Jobs.Dtos;

namespace JobSearchSiteBackend.API.Controllers.Jobs.Dtos;

public record GetJobResponse(JobDetailedDto Job);
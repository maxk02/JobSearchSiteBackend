using JobSearchSiteBackend.Core.Domains.JobContractTypes.Dtos;
using JobSearchSiteBackend.Core.Domains.Jobs.Dtos;
using JobSearchSiteBackend.Core.Domains.Locations.Dtos;

namespace JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetJobById;

public record GetJobByIdResponse(JobDetailedDto Job);
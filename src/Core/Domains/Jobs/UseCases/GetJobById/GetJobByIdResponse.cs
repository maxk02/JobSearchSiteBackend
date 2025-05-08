using Core.Domains.JobContractTypes.Dtos;
using Core.Domains.Jobs.Dtos;
using Core.Domains.Locations.Dtos;

namespace Core.Domains.Jobs.UseCases.GetJobById;

public record GetJobByIdResponse(JobDetailedDto Job);
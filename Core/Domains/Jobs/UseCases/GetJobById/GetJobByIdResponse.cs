using Core.Domains._Shared.ValueEntities;
using Core.Domains.JobContractTypes.Dtos;
using Core.Domains.Locations.Dtos;

namespace Core.Domains.Jobs.UseCases.GetJobById;

public record GetJobByIdResponse(
    long Id,
    long CategoryId,
    string Title,
    string Description,
    DateTime DateTimePublishedUtc,
    DateTime DateTimeExpiringUtc,
    ICollection<string> Responsibilities,
    ICollection<string> Requirements,
    ICollection<string> Advantages,
    SalaryRecord? SalaryRecord,
    EmploymentTypeRecord? EmploymentTypeRecord,
    ICollection<JobContractTypeDto> JobContractTypes,
    ICollection<LocationDto> Locations);
namespace JobSearchSiteBackend.API.Controllers.Jobs.Dtos;

public record GetJobsRequest(
    string? Query,
    int Page,
    int Size,
    bool? MustHaveSalaryRecord,
    ICollection<long>? EmploymentOptionIds,
    ICollection<long>? LocationIds,
    ICollection<long>? CategoryIds,
    ICollection<long>? ContractTypeIds);
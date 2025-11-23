namespace JobSearchSiteBackend.API.Controllers.Jobs.Dtos;

public record GetJobsRequest(
    string Query,
    int Page,
    int Size,
    bool? MustHaveSalaryRecord,
    ICollection<long>? EmploymentTypeIds,
    ICollection<long>? CountryIds,
    ICollection<long>? CategoryIds,
    ICollection<long>? ContractTypeIds);
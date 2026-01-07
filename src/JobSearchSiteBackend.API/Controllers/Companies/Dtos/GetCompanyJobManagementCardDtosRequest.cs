namespace JobSearchSiteBackend.API.Controllers.Companies.Dtos;

public record GetCompanyJobManagementCardDtosRequest(
    string? Query,
    int Page,
    int Size,
    bool? MustHaveSalaryRecord,
    long? LocationId,
    ICollection<long>? EmploymentTypeIds,
    ICollection<long>? CategoryIds,
    ICollection<long>? ContractTypeIds);
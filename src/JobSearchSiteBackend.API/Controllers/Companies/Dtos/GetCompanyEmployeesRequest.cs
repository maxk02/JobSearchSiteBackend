namespace JobSearchSiteBackend.API.Controllers.Companies.Dtos;

public record GetCompanyEmployeesRequest(string? Query, int Page, int Size);
using JobSearchSiteBackend.Main.Domains._Shared.Pagination;
using JobSearchSiteBackend.Main.Domains.Companies.Dtos;

namespace JobSearchSiteBackend.Main.Domains.Companies.UseCases.GetCompanyEmployees;

public record GetCompanyEmployeesResult(ICollection<CompanyEmployeeDto> Employees, PaginationResponse PaginationResponse);
using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains.Companies.Dtos;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyEmployees;

public record GetCompanyEmployeesResponse(ICollection<CompanyEmployeeDto> Employees, PaginationResponse PaginationResponse);
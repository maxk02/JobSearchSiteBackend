using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains.Companies.Dtos;

namespace JobSearchSiteBackend.API.Controllers.Companies.Dtos;

public record GetCompanyEmployeesResponse(ICollection<CompanyEmployeeDto> Employees, PaginationResponse PaginationResponse);
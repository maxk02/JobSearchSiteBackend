using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompany;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyEmployees;

public record GetCompanyEmployeesQuery(long CompanyId, string? Query, int Page, int Size) : IRequest<Result<GetCompanyEmployeesResult>>;
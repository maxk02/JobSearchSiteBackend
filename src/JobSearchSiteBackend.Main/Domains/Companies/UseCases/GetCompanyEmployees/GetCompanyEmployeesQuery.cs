using Ardalis.Result;
using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Main.Domains.Companies.UseCases.GetCompany;

namespace JobSearchSiteBackend.Main.Domains.Companies.UseCases.GetCompanyEmployees;

public record GetCompanyEmployeesQuery(long CompanyId, string? Query, int Page, int Size) : IRequest<Result<GetCompanyEmployeesResult>>;
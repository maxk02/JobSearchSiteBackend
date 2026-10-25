using Ardalis.Result;
using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Main.Domains.Companies.UseCases.GetCompanyJobs;

namespace JobSearchSiteBackend.Main.Domains.Companies.UseCases.GetCompanyManagementNavbarDto;

public record GetCompanyManagementNavbarDtoQuery(long CompanyId) : IRequest<Result<GetCompanyManagementNavbarDtoResult>>;
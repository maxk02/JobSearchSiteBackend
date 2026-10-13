using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyJobs;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyManagementNavbarDto;

public record GetCompanyManagementNavbarDtoQuery(long CompanyId) : IRequest<Result<GetCompanyManagementNavbarDtoResult>>;
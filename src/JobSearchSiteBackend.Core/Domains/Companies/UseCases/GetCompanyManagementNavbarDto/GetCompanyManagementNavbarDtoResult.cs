using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains.Companies.Dtos;
using JobSearchSiteBackend.Core.Domains.Jobs.Dtos;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyManagementNavbarDto;

public record GetCompanyManagementNavbarDtoResult(CompanyManagementDetailedDto Company);
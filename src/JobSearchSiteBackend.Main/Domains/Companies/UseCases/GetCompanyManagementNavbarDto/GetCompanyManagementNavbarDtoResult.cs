using JobSearchSiteBackend.Main.Domains._Shared.Pagination;
using JobSearchSiteBackend.Main.Domains.Companies.Dtos;
using JobSearchSiteBackend.Main.Domains.Jobs.Dtos;

namespace JobSearchSiteBackend.Main.Domains.Companies.UseCases.GetCompanyManagementNavbarDto;

public record GetCompanyManagementNavbarDtoResult(CompanyManagementDetailedDto Company);
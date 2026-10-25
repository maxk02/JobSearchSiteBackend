using JobSearchSiteBackend.Main.Domains.Companies.Dtos;

namespace JobSearchSiteBackend.Main.Domains.Companies.UseCases.GetCompanyLastVisitedJobs;

public record GetCompanyLastVisitedJobsResult(ICollection<CompanyJobListItemDto> Jobs, CompanyDashboardStatsDto? DashboardStatsDto);
namespace JobSearchSiteBackend.Core.Domains.Companies.Dtos;

public record CompanyDashboardStatsDto(long JobViewsToday, long JobViewsLastWeek,
    long JobApplicationsToday, long JobApplicationsLastWeek);
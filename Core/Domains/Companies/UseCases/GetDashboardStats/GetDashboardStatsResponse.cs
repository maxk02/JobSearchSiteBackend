namespace Core.Domains.Companies.UseCases.GetDashboardStats;

public record GetDashboardStatsResponse(int JobsVisitedLastDay, int JobsVisitedLastWeek,
    int ApplicationsMadeLastDay, int ApplicationsMadeLastWeek);
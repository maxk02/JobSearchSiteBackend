namespace JobSearchSiteBackend.API.Controllers.Jobs.Dtos;

public record GetDailyApplicationsForJobForDateRangeResponse(List<Dictionary<DateTime, long>> DailyApplications);
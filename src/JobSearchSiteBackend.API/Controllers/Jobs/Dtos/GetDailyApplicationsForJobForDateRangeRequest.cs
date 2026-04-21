namespace JobSearchSiteBackend.API.Controllers.Jobs.Dtos;

public record GetDailyApplicationsForJobForDateRangeRequest(DateTime StartDate, DateTime EndDate);
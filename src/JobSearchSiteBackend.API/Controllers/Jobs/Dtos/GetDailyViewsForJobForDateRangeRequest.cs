namespace JobSearchSiteBackend.API.Controllers.Jobs.Dtos;

public record GetDailyViewsForJobForDateRangeRequest(DateTime StartDate, DateTime EndDate);
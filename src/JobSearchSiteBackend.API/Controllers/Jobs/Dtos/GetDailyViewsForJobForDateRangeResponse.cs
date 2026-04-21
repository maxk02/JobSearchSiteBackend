namespace JobSearchSiteBackend.API.Controllers.Jobs.Dtos;

public record GetDailyViewsForJobForDateRangeResponse(List<Dictionary<DateTime, long>> DailyViews);
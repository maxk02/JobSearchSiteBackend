namespace JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetDailyViewsForJobForDateRange;

public record GetDailyViewsForJobForDateRangeResult(List<Dictionary<DateTime, long>> DailyViews);
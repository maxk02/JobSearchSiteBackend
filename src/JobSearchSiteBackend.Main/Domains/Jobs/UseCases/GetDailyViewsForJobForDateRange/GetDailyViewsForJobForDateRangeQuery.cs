using Ardalis.Result;
using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Main.Domains.Jobs.UseCases.GetDailyViewsForJobForDateRange;

public record GetDailyViewsForJobForDateRangeQuery(long Id, DateTime StartDate, DateTime EndDate)
    : IRequest<Result<GetDailyViewsForJobForDateRangeResult>>;
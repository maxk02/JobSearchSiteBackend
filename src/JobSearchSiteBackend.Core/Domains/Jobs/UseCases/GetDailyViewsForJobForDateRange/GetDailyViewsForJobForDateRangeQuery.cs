using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetDailyViewsForJobForDateRange;

public record GetDailyViewsForJobForDateRangeQuery(long Id, DateTime StartDate, DateTime EndDate)
    : IRequest<Result<GetDailyViewsForJobForDateRangeResult>>;
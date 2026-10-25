using Ardalis.Result;
using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Main.Domains.Jobs.UseCases.GetDailyApplicationsForJobForDateRange;

public record GetDailyApplicationsForJobForDateRangeQuery(long Id, DateTime StartDate, DateTime EndDate)
    : IRequest<Result<GetDailyApplicationsForJobForDateRangeResult>>;
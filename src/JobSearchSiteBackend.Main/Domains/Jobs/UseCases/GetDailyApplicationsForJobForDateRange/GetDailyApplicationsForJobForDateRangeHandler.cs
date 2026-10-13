using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.CompanyClaims;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.Caching;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetDailyApplicationsForJobForDateRange;

public class GetDailyApplicationsForJobForDateRangeHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context)
    : IRequestHandler<GetDailyApplicationsForJobForDateRangeQuery, Result<GetDailyApplicationsForJobForDateRangeResult>>
{
    public async Task<Result<GetDailyApplicationsForJobForDateRangeResult>> Handle(GetDailyApplicationsForJobForDateRangeQuery query,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var companyId = await context.Jobs
            .Where(j => j.Id == query.Id)
            .Select(j => j.CompanyId)
            .SingleOrDefaultAsync(cancellationToken);

        var hasPermissionInRequestedCompany =
            await context.UserCompanyClaims
                .Where(ucc => ucc.CompanyId == companyId
                              && ucc.UserId == currentUserId
                              && ucc.ClaimId == CompanyClaim.CanReadStats.Id)
                .AnyAsync(cancellationToken);

        if (!hasPermissionInRequestedCompany)
            return Result.Forbidden();

        var dbResults = await context.JobApplications
            .Where(x => x.JobId == query.Id)
            .Where(x => x.DateTimeCreatedUtc >= query.StartDate && x.DateTimeCreatedUtc <= query.EndDate)
            .GroupBy(x => x.DateTimeCreatedUtc.Date)
            .Select(g => new 
            {
                Date = g.Key,
                Count = g.Count()
            })
            .ToDictionaryAsync(x => x.Date, x => x.Count, cancellationToken);
        
        var dailyApplicationsDictionaryList = new List<Dictionary<DateTime, long>>();

        for (var current = query.StartDate.Date; current <= query.EndDate.Date; current = current.AddDays(1))
        {
            // default to 0
            dbResults.TryGetValue(current, out int countForDay);

            dailyApplicationsDictionaryList.Add(new Dictionary<DateTime, long>
            {
                {current, countForDay}
            });
        }

        var result = new GetDailyApplicationsForJobForDateRangeResult(dailyApplicationsDictionaryList);
        
        return Result.Success(result);
    }
}
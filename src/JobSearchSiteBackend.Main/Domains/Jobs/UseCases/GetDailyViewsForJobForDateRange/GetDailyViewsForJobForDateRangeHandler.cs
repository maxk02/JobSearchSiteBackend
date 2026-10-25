using Ardalis.Result;
using JobSearchSiteBackend.Main.Domains._Shared.Pagination;
using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Main.Domains.CompanyClaims;
using JobSearchSiteBackend.Main.Domains.JobApplications.Dtos;
using JobSearchSiteBackend.Main.Domains.JobApplications.Search;
using JobSearchSiteBackend.Main.Domains.Locations;
using JobSearchSiteBackend.Main.Domains.PersonalFiles;
using JobSearchSiteBackend.Main.Domains.UserProfiles.Persistence;
using JobSearchSiteBackend.Main.Persistence;
using JobSearchSiteBackend.Main.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Main.Domains.Jobs.UseCases.GetDailyViewsForJobForDateRange;

public class GetDailyViewsForJobForDateRangeHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context,
    IJobPageVisitCacheRepository jobPageVisitCacheRepo)
    : IRequestHandler<GetDailyViewsForJobForDateRangeQuery, Result<GetDailyViewsForJobForDateRangeResult>>
{
    public async Task<Result<GetDailyViewsForJobForDateRangeResult>> Handle(GetDailyViewsForJobForDateRangeQuery query,
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
        
        var viewsDictionary = await jobPageVisitCacheRepo.GetJobViewsForDateRangeAsync(companyId.ToString(),
            query.Id.ToString(), query.StartDate, query.EndDate);
        
        var listOfOneObjectDictionaries = viewsDictionary
            .Select(kvp => new Dictionary<DateTime, long>
            {
                { kvp.Key, kvp.Value }
            })
            .ToList();

        var result = new GetDailyViewsForJobForDateRangeResult(listOfOneObjectDictionaries);
        
        return Result.Success(result);
    }
}
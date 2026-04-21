using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.CompanyClaims;
using JobSearchSiteBackend.Core.Domains.JobApplications.Dtos;
using JobSearchSiteBackend.Core.Domains.JobApplications.Search;
using JobSearchSiteBackend.Core.Domains.Locations;
using JobSearchSiteBackend.Core.Domains.PersonalFiles;
using JobSearchSiteBackend.Core.Domains.UserProfiles.Persistence;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.Caching;
using JobSearchSiteBackend.Core.Services.FileStorage;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetDailyViewsForJobForDateRange;

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
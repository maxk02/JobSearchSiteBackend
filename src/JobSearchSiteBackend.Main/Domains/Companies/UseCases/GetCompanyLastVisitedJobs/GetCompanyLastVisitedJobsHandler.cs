using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Companies.Dtos;
using JobSearchSiteBackend.Core.Domains.CompanyClaims;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.Caching;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyLastVisitedJobs;

public class GetCompanyLastVisitedJobsHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context,
    ICompanyLastVisitedJobsCacheRepository cacheRepo,
    IJobPageVisitCacheRepository jobPageVisitCacheRepo)
    : IRequestHandler<GetCompanyLastVisitedJobsQuery, Result<GetCompanyLastVisitedJobsResult>>
{
    public async Task<Result<GetCompanyLastVisitedJobsResult>> Handle(GetCompanyLastVisitedJobsQuery query,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        List<CompanyJobListItemDto> lastVisitedJobs = [];
        CompanyDashboardStatsDto? companyDashboardStats = null;
        
        var claimIdsInCompany =
            await context.UserCompanyClaims
                .Where(ucc => ucc.CompanyId == query.CompanyId
                              && ucc.UserId == currentUserId)
                .Select(c => c.Id)
                .ToListAsync(cancellationToken);
        
        if (claimIdsInCompany.Count == 0)
            return Result.Forbidden();

        if (claimIdsInCompany.Contains(CompanyClaim.CanEditJobs.Id))
        {
            var idListFromCache = await cacheRepo
                .GetLastVisitedAsync(currentUserId.ToString(), query.CompanyId.ToString());
        
            var jobListItemDtos = await context.Jobs
                .Where(job => job.CompanyId == query.CompanyId)
                .Where(jobListItem => idListFromCache.Contains(jobListItem.Id))
                .Select(job => new CompanyJobListItemDto(job.Id, job.Title))
                .ToListAsync(cancellationToken);

            lastVisitedJobs = jobListItemDtos;
        }

        if (claimIdsInCompany.Contains(CompanyClaim.CanReadStats.Id))
        {
            var jobViewsToday = await jobPageVisitCacheRepo.GetCumulatedCompanyViewsForDateAsync(query.CompanyId.ToString(), DateTime.Today);
            var jobViewsLastWeek = await jobPageVisitCacheRepo.GetTotalCompanyViewsForDateRangeAsync(query.CompanyId.ToString(), DateTime.Today.AddDays(-7), DateTime.Today);

            var jobApplicationsToday = await context.JobApplications
                .CountAsync(ja => ja.Job!.CompanyId == query.CompanyId
                                  && ja.DateTimeCreatedUtc.Date == DateTime.Today, cancellationToken);
            
            var jobApplicationsLastWeek = await context.JobApplications
                .CountAsync(ja => ja.Job!.CompanyId == query.CompanyId
                                  && ja.DateTimeCreatedUtc.Date >= DateTime.Today.AddDays(-7)
                                  && ja.DateTimeCreatedUtc.Date <= DateTime.Today, cancellationToken);

            companyDashboardStats = new CompanyDashboardStatsDto(jobViewsToday, jobViewsLastWeek, jobApplicationsToday, jobApplicationsLastWeek);
        }

        var result = new GetCompanyLastVisitedJobsResult(lastVisitedJobs, companyDashboardStats);

        return Result.Success(result);
    }
}
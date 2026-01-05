using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Companies.Dtos;
using JobSearchSiteBackend.Core.Domains.CompanyClaims;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.SearchCompanySharedJobs;

public class SearchCompanySharedJobsHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<SearchCompanySharedJobsQuery, Result<SearchCompanySharedJobsResult>>
{
    public async Task<Result<SearchCompanySharedJobsResult>> Handle(SearchCompanySharedJobsQuery query,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var hasPermissionInRequestedCompany =
            await context.UserCompanyClaims
                .Where(ucc => ucc.CompanyId == query.CompanyId
                    && ucc.UserId == currentUserId
                    && ucc.ClaimId == CompanyClaim.CanEditJobs.Id)
                .AnyAsync();

        if (!hasPermissionInRequestedCompany)
            return Result.Forbidden();

        var jobListItemDtos = await context.Jobs
            .Where(job => job.CompanyId == query.CompanyId)
            .Where(jobItem => jobItem.Title.ToLower().Contains(query.Query.ToLower()))
            .Select(job => new CompanyJobListItemDto(job.Id, job.Title))
            .ToListAsync(cancellationToken);
        
        var response = new SearchCompanySharedJobsResult(jobListItemDtos);
        
        return Result.Success(response);
    }
}
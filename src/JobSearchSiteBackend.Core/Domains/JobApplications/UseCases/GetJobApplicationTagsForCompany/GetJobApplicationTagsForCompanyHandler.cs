using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.CompanyClaims;
using JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.AddJobApplicationTag;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.GetJobApplicationTagsForCompany;

public class GetJobApplicationTagsForCompanyHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context)
    : IRequestHandler<GetJobApplicationTagsForCompanyQuery, Result<GetJobApplicationTagsForCompanyResult>>
{
    public async Task<Result<GetJobApplicationTagsForCompanyResult>> Handle(GetJobApplicationTagsForCompanyQuery query,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();
        
        var hasPermissionInRequestedCompany =
            await context.UserCompanyClaims
                .Where(ucc => ucc.CompanyId == query.CompanyId
                              && ucc.UserId == currentUserId
                              && ucc.ClaimId == CompanyClaim.CanManageApplications.Id)
                .AnyAsync(cancellationToken);

        if (!hasPermissionInRequestedCompany)
            return Result.Forbidden();
        
        var jobApplicationTagsQuery = context.JobApplications
            .Where(ja => ja.Job!.CompanyId == query.CompanyId)
            .SelectMany(ja => ja.Tags!)
            .Select(t => t.Tag);

        if (!string.IsNullOrEmpty(query.SearchQuery))
        {
            jobApplicationTagsQuery = jobApplicationTagsQuery.Where(t => t.ToLower().Contains(query.SearchQuery.ToLower()));
        }

        jobApplicationTagsQuery = jobApplicationTagsQuery.Distinct();
        
        var count = await jobApplicationTagsQuery.CountAsync(cancellationToken);

        var jobApplicationTags = await jobApplicationTagsQuery
            .Take(query.Size)
            .ToListAsync(cancellationToken);

        var paginationResponse = new PaginationResponse(1, query.Size, count);

        var result = new GetJobApplicationTagsForCompanyResult(jobApplicationTags, paginationResponse);
        
        return Result.Success(result);
    }
}
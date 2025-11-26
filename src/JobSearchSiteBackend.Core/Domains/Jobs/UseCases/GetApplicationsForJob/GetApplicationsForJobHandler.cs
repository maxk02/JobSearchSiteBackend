using Ardalis.Result;
using AutoMapper;
using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.JobApplications.Dtos;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims;
using JobSearchSiteBackend.Core.Domains.JobFolders;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.Caching;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetApplicationsForJob;

public class GetApplicationsForJobHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context,
    ICompanyLastVisitedJobsCacheRepository cacheRepo,
    IMapper mapper) : IRequestHandler<GetApplicationsForJobQuery, Result<GetApplicationsForJobResult>>
{
    public async Task<Result<GetApplicationsForJobResult>> Handle(GetApplicationsForJobQuery query,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var dbQuery = context.JobApplications
            .Where(ja => ja.JobId == query.Id);

        if (query.StatusIds.Count > 0)
        {
            dbQuery = dbQuery
                .Where(ja => query.StatusIds.Contains((int)ja.Status));
        }

        if (query.IncludedTags.Count > 0)
        {
            dbQuery = dbQuery
                .Where(ja => ja.Tags!.Any(t => query.IncludedTags.Contains(t.Tag)));
        }
        
        if (query.ExcludedTags.Count > 0)
        {
            dbQuery = dbQuery
                .Where(ja => !ja.Tags!.Any(t => query.IncludedTags.Contains(t.Tag)));
        }
        
        var totalCount = await dbQuery.CountAsync(cancellationToken);

        dbQuery = dbQuery
            .Skip((query.Page - 1) * query.Size)
            .Take(query.Size);
        
        var jobApplicationsWithJobFolder =  await dbQuery
            .GroupBy(ja => ja.Job!.JobFolder!)
            .Select(g => new
            {
                JobFolder = g.Key,
                JobApplications = g.ToList()
            })
            .SingleOrDefaultAsync(cancellationToken);

        if (jobApplicationsWithJobFolder is null)
        {
            return Result.NotFound();
        }
        
        var canEdit = await context.JobFolderRelations
            .GetThisOrAncestorWhereUserHasClaim(jobApplicationsWithJobFolder.JobFolder.Id, currentUserId,
                JobFolderClaim.CanManageApplications.Id)
            .AnyAsync(cancellationToken);

        if (!canEdit)
            return Result.Forbidden();
        
        await cacheRepo.AddLastVisitedAsync(currentUserId.ToString(),
            jobApplicationsWithJobFolder.JobFolder.CompanyId.ToString(), query.Id.ToString());

        if (jobApplicationsWithJobFolder.JobApplications.Count == 0)
            return Result<GetApplicationsForJobResult>.NoContent();
        
        var jobApplicationDtos = mapper.Map<List<JobApplicationForManagersDto>>(jobApplicationsWithJobFolder.JobApplications);

        var paginationResponse = new PaginationResponse(query.Page, query.Size, totalCount);
        
        var result = new GetApplicationsForJobResult(jobApplicationDtos, paginationResponse);
        
        return Result.Success(result);
    }
}
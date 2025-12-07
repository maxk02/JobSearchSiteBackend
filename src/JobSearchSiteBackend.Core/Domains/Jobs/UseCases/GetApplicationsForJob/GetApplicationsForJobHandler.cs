using Ardalis.Result;
using AutoMapper;
using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.JobApplications.Dtos;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims;
using JobSearchSiteBackend.Core.Domains.JobFolders;
using JobSearchSiteBackend.Core.Domains.JobFolders.Persistence;
using JobSearchSiteBackend.Core.Domains.PersonalFiles;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.Caching;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetApplicationsForJob;

public class GetApplicationsForJobHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context,
    ICompanyLastVisitedJobsCacheRepository cacheRepo) : IRequestHandler<GetApplicationsForJobQuery, Result<GetApplicationsForJobResult>>
{
    public async Task<Result<GetApplicationsForJobResult>> Handle(GetApplicationsForJobQuery query,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var dbQuery = context.JobApplications
            .Where(ja => ja.JobId == query.Id);

        if (!string.IsNullOrEmpty(query.Query))
        {
            dbQuery = dbQuery
                .Where(ja => ja.User!.FirstName.ToLower().Contains(query.Query.ToLower())
                || ja.User.LastName.ToLower().Contains(query.Query.ToLower()));
        }
        
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

        if (string.IsNullOrEmpty(query.SortOption) || query.SortOption == "dateAppliedDesc")
        {
            dbQuery = dbQuery.OrderByDescending(ja => ja.DateTimeCreatedUtc);
        }
        else if (query.SortOption == "dateAppliedAsc")
        {
            dbQuery = dbQuery.OrderBy(ja => ja.DateTimeCreatedUtc);
        }
        
        dbQuery = dbQuery
            .Skip((query.Page - 1) * query.Size)
            .Take(query.Size);

        var jobApplicationDbQueryItem = await dbQuery
            .GroupBy(ja => ja.Job!.JobFolder!)
            .Select(g => new
            {
                HasManageApplicationsPermission = g.Key.RelationsWhereThisIsDescendant!
                    .SelectMany(rel => rel.Ancestor!.UserJobFolderClaims!
                        .Where(ujfc => ujfc.UserId == currentUserId && ujfc.ClaimId == JobFolderClaim.CanManageApplications.Id))
                    .Any(),
                CompanyId = g.Key.CompanyId,
                JobApplicationItems = g.Select(ja => new
                {
                    Id = ja.Id,
                    UserId = ja.UserId,
                    UserFullName = ja.User!.FirstName + " " + ja.User!.LastName,
                    DateTimeAppliedUtc = ja.DateTimeCreatedUtc,
                    PersonalFiles = ja.PersonalFiles,
                    Status = ja.Status
                })
            })
            .SingleOrDefaultAsync(cancellationToken);

        if (jobApplicationDbQueryItem is null)
        {
            return Result.NotFound();
        }

        if (!jobApplicationDbQueryItem.HasManageApplicationsPermission)
        {
            return Result.Forbidden();
        }
        
        await cacheRepo.AddLastVisitedAsync(currentUserId.ToString(),
            jobApplicationDbQueryItem.CompanyId.ToString(), query.Id.ToString());
        
        var jobApplicationDtos = jobApplicationDbQueryItem.JobApplicationItems
            .Select(jaItem => new JobApplicationForManagersDto(
                jaItem.Id,
                jaItem.UserId,
                jaItem.UserFullName,
                jaItem.DateTimeAppliedUtc,
                jaItem.PersonalFiles!.Select(pf => pf.ToPersonalFileInfoDto()).ToList(),
                jaItem.Status
                ))
            .ToList();

        var paginationResponse = new PaginationResponse(query.Page, query.Size, totalCount);
        
        var result = new GetApplicationsForJobResult(jobApplicationDtos, paginationResponse);
        
        return Result.Success(result);
    }
}
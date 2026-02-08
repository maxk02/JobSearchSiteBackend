using Ardalis.Result;
using AutoMapper;
using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.CompanyClaims;
using JobSearchSiteBackend.Core.Domains.JobApplications.Dtos;
using JobSearchSiteBackend.Core.Domains.Locations;
using JobSearchSiteBackend.Core.Domains.PersonalFiles;
using JobSearchSiteBackend.Core.Domains.UserProfiles.Persistence;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.Caching;
using JobSearchSiteBackend.Core.Services.FileStorage;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetApplicationsForJob;

public class GetApplicationsForJobHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context,
    ICompanyLastVisitedJobsCacheRepository cacheRepo,
    IFileStorageService fileStorageService)
    : IRequestHandler<GetApplicationsForJobQuery, Result<GetApplicationsForJobResult>>
{
    public async Task<Result<GetApplicationsForJobResult>> Handle(GetApplicationsForJobQuery query,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var companyId = await context.Jobs
            .Where(j => j.Id == query.Id)
            .Select(j => j.CompanyId)
            .SingleOrDefaultAsync(cancellationToken);

        var hasManageApplicationsPermission = await context.UserCompanyClaims
            .Where(ucc => ucc.ClaimId == CompanyClaim.CanManageApplications.Id)
            .Where(ucc => ucc.CompanyId == companyId)
            .Where(ucc => ucc.UserId == currentUserId)
            .AnyAsync(cancellationToken);

        if (!hasManageApplicationsPermission)
        {
            return Result.Forbidden();
        }

        var dbQuery = context.JobApplications
            .Where(ja => ja.JobId == query.Id)
            .Where(ja => ja.LocationId == query.LocationId);

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
                .Where(ja => !ja.Tags!.Any(t => query.ExcludedTags.Contains(t.Tag)));
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

        var jobApplicationItems = await dbQuery
            .Select(ja => new
            {
                Id = ja.Id,
                UserId = ja.UserId,
                UserFullName = ja.User!.FirstName + " " + ja.User!.LastName,
                UserAvatars = ja.User!.UserAvatars!,
                Email = ja.User!.Account!.Email!,
                Phone = ja.User!.Phone,
                Tags = ja.Tags!,
                DateTimeAppliedUtc = ja.DateTimeCreatedUtc,
                PersonalFiles = ja.PersonalFiles,
                Status = ja.Status,
                Location = ja.Location
            })
            .ToListAsync(cancellationToken);
        
        if (jobApplicationItems.Count == 0)
            return Result.NotFound();

        List<JobApplicationForManagersDto> jobApplicationForManagersDtos = [];
        
        foreach (var jaItem in jobApplicationItems)
        {
            var avatar = jaItem.UserAvatars.ToList().GetLatestAvailableAvatar();

            string? avatarLink = null;

            if (avatar is not null)
            {
                avatarLink = await fileStorageService.GetDownloadUrlAsync(FileStorageBucketName.UserAvatars, 
                    avatar.GuidIdentifier, avatar.Extension, cancellationToken);
            }

            var jobApplicationDto = new JobApplicationForManagersDto(
                jaItem.Id,
                jaItem.UserId,
                jaItem.UserFullName,
                avatarLink,
                jaItem.Email,
                jaItem.Phone,
                jaItem.Tags.Select(t => t.Tag).ToList(),
                jaItem.DateTimeAppliedUtc,
                jaItem.PersonalFiles!.Select(pf => pf.ToPersonalFileInfoDto()).ToList(),
                (int)jaItem.Status,
                jaItem.Location!.ToLocationDto()
            );
            
            jobApplicationForManagersDtos.Add(jobApplicationDto);
        }
        
        await cacheRepo.AddLastVisitedAsync(currentUserId.ToString(),
            companyId.ToString(), query.Id.ToString());

        var paginationResponse = new PaginationResponse(query.Page, query.Size, totalCount);
        
        var result = new GetApplicationsForJobResult(jobApplicationForManagersDtos, paginationResponse);
        
        return Result.Success(result);
    }
}
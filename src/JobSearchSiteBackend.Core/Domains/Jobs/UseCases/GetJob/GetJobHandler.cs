using Ardalis.Result;
using AutoMapper;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Companies.Persistence;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims;
using JobSearchSiteBackend.Core.Domains.JobFolders;
using JobSearchSiteBackend.Core.Domains.Jobs.Dtos;
using JobSearchSiteBackend.Core.Domains.Locations;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.Caching;
using JobSearchSiteBackend.Core.Services.FileStorage;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetJob;

public class GetJobHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context,
    IPageVisitCacheRepository cacheRepo,
    IFileStorageService fileStorageService) : IRequestHandler<GetJobQuery, Result<GetJobResult>>
{
    public async Task<Result<GetJobResult>> Handle(GetJobQuery query,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetId();
        
        var dbQuery = context.Jobs
            .AsNoTracking()
            .Where(j => j.Id == query.Id)
            .Include(j => j.JobFolder)
            .ThenInclude(jf => jf!.Company)
            .ThenInclude(c => c!.CompanyAvatars)
            .Include(job => job.SalaryInfo)
            .Include(job => job.EmploymentOptions)
            // .Include(job => job.Responsibilities)
            // .Include(job => job.Requirements)
            // .Include(job => job.NiceToHaves)
            .Include(job => job.JobContractTypes)
            .Include(job => job.Locations);

        Job? fetchedJob = null;
        var isBookmarked = false;
        long? applicationId = null;
        
        if (currentUserId is not null)
        {
            fetchedJob = await dbQuery
                .Include(job => job.JobApplications!.Where(ja => ja.UserId == currentUserId.Value))
                .SingleOrDefaultAsync(cancellationToken);
            
            if (fetchedJob is null)
                return Result.NotFound();

            applicationId = fetchedJob.JobApplications?.FirstOrDefault()?.Id;
            
            isBookmarked = await context.Jobs
                .Where(j => j.Id == query.Id)
                .Where(j => j.UserJobBookmarks!.Any(u => u.UserId == currentUserId.Value))
                .AnyAsync(cancellationToken);
        }
        else
        {
            fetchedJob = await dbQuery.SingleOrDefaultAsync(cancellationToken);
        }

        if (fetchedJob is null)
            return Result.NotFound();
        
        if (!fetchedJob.IsPublic)
            return Result.Forbidden();

        await cacheRepo.IncrementJobVisitsCounterAsync(fetchedJob.JobFolder!.CompanyId.ToString(), fetchedJob.Id.ToString());
        
        var avatar = fetchedJob.JobFolder?.Company?.CompanyAvatars?.GetLatestAvailableAvatar();

        string? avatarLink = null;

        if (avatar is not null)
        {
            avatarLink = await fileStorageService.GetDownloadUrlAsync(FileStorageBucketName.CompanyAvatars, 
                avatar.GuidIdentifier, avatar.Extension, cancellationToken);
        }
        
        var jobDetailedDto = new JobDetailedDto(
            fetchedJob.Id,
            fetchedJob.JobFolder!.CompanyId,
            avatarLink,
            fetchedJob.JobFolder!.Company!.Name,
            fetchedJob.JobFolder.Company.Description,
            fetchedJob.Locations!.Select(l => l.ToLocationDto()).ToList(),
            fetchedJob.CategoryId,
            fetchedJob.Title,
            fetchedJob.Description,
            fetchedJob.DateTimePublishedUtc,
            fetchedJob.DateTimeExpiringUtc,
            fetchedJob.Responsibilities!,
            fetchedJob.Requirements!,
            fetchedJob.NiceToHaves!,
            fetchedJob.SalaryInfo!.ToJobSalaryInfoDto(),
            fetchedJob.EmploymentOptions!.Select(eo => eo.Id).ToList(),
            fetchedJob.JobContractTypes!.Select(ct => ct.Id).ToList(),
            isBookmarked,
            applicationId);
        
        return new GetJobResult(jobDetailedDto);
    }
}
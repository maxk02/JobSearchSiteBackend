using Ardalis.Result;
using AutoMapper;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims;
using JobSearchSiteBackend.Core.Domains.JobFolders;
using JobSearchSiteBackend.Core.Domains.JobFolders.Persistence;
using JobSearchSiteBackend.Core.Domains.Jobs.Dtos;
using JobSearchSiteBackend.Core.Domains.Locations;
using JobSearchSiteBackend.Core.Domains.Locations.Dtos;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.Caching;
using JobSearchSiteBackend.Core.Services.FileStorage;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetJobManagementDto;

public class GetJobManagementDtoHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context,
    IFileStorageService fileStorageService,
    ICompanyLastVisitedJobsCacheRepository cacheRepo) : IRequestHandler<GetJobManagementDtoQuery, Result<GetJobManagementDtoResult>>
{
    public async Task<Result<GetJobManagementDtoResult>> Handle(GetJobManagementDtoQuery query,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();
        
        var job = await context.Jobs
            .AsNoTracking()
            .Where(j => j.Id == query.Id)
            .Include(job => job.JobFolder)
            .ThenInclude(jf => jf!.Company)
            .ThenInclude(c => c!.CompanyAvatars!
                .Where(a => !a.IsDeleted && a.IsUploadedSuccessfully)
                .OrderBy(a => a.DateTimeUpdatedUtc))
            .Include(job => job.SalaryInfo)
            .Include(job => job.EmploymentOptions)
            .Include(job => job.Responsibilities)
            .Include(job => job.Requirements)
            .Include(job => job.NiceToHaves)
            .Include(job => job.JobContractTypes)
            .Include(job => job.Locations)
            .SingleOrDefaultAsync(cancellationToken);

        if (job is null)
            return Result<GetJobManagementDtoResult>.NotFound();

        var claimIdsForCurrentUser = context.JobFolderRelations
            .GetClaimIdsForThisAndAncestors(job.JobFolderId, currentUserId)
            .ToList();

        if (!claimIdsForCurrentUser.Contains(JobFolderClaim.CanEditJobs.Id))
            return Result.Forbidden();
        
        await cacheRepo.AddLastVisitedAsync(currentUserId.ToString(),
            job.JobFolder!.CompanyId.ToString(), job.Id.ToString());
        
        var lastAvatar = job.JobFolder.Company!.CompanyAvatars!.LastOrDefault();
        string? companyLogoLink = null;
        
        if (lastAvatar is not null)
        {
            companyLogoLink = await fileStorageService.GetDownloadUrlAsync(FileStorageBucketName.CompanyAvatars,
                lastAvatar.GuidIdentifier, lastAvatar.Extension, cancellationToken);
        }

        var jobManagementDto = new JobManagementDto(
            job.Id,
            job.JobFolder!.CompanyId,
            companyLogoLink,
            job.JobFolder!.Company!.Name,
            job.JobFolder.Company.Description,
            job.Locations!.Select(l => l.ToLocationDto()).ToList(),
            job.CategoryId,
            job.Title,
            job.Description,
            job.DateTimePublishedUtc,
            job.DateTimeExpiringUtc,
            job.Responsibilities!,
            job.Requirements!,
            job.NiceToHaves!,
            job.SalaryInfo?.ToJobSalaryInfoDto(),
            job.EmploymentOptions!.Select(x => x.Id).ToList(),
            job.JobContractTypes!.Select(x => x.Id).ToList(),
            job.JobFolderId,
            job.JobFolder.Name!,
            claimIdsForCurrentUser,
            job.IsPublic,
            job.TimeRangeOptionId
            );
        
        return new GetJobManagementDtoResult(jobManagementDto);
    }
}
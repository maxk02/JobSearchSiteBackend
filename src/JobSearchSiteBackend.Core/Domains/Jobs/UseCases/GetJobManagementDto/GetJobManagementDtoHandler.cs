using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.CompanyClaims;
using JobSearchSiteBackend.Core.Domains.Jobs.Dtos;
using JobSearchSiteBackend.Core.Domains.Locations;
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
            .Include(jf => jf!.Company)
            .ThenInclude(c => c!.CompanyAvatars!
                .Where(a => !a.IsDeleted && a.IsUploadedSuccessfully)
                .OrderBy(a => a.DateTimeUpdatedUtc))
            .Include(job => job.SalaryInfo)
            .Include(job => job.EmploymentOptions)
            .Include(job => job.JobContractTypes)
            .Include(job => job.Locations)
            .SingleOrDefaultAsync(cancellationToken);

        if (job is null)
            return Result<GetJobManagementDtoResult>.NotFound();

        var claimIdsForCurrentUser = await context.UserCompanyClaims
            .Where(ucc => ucc.CompanyId == job.CompanyId && ucc.UserId == currentUserId)
            .Select(ucc => ucc.ClaimId)
            .ToListAsync();

        var hasPermissionInRequestedCompany = claimIdsForCurrentUser.Contains(CompanyClaim.CanReadJobs.Id);

        if (!hasPermissionInRequestedCompany)
            return Result.Forbidden();
        
        await cacheRepo.AddLastVisitedAsync(currentUserId.ToString(),
            job.CompanyId.ToString(), job.Id.ToString());
        
        var lastAvatar = job.Company!.CompanyAvatars!.LastOrDefault();
        string? companyLogoLink = null;
        
        if (lastAvatar is not null)
        {
            companyLogoLink = await fileStorageService.GetDownloadUrlAsync(FileStorageBucketName.CompanyAvatars,
                lastAvatar.GuidIdentifier, lastAvatar.Extension, cancellationToken);
        }

        var jobManagementDto = new JobManagementDto(
            job.Id,
            job.CompanyId,
            companyLogoLink,
            job.Company!.Name,
            job.Company.Description,
            job.Locations!.Select(l => l.ToLocationDto()).ToList(),
            job.CategoryId,
            job.Title,
            job.Description,
            job.DateTimePublishedUtc,
            job.DateTimeExpiringUtc,
            job.MaxDateTimeExpiringUtcEverSet,
            job.Responsibilities!,
            job.Requirements!,
            job.NiceToHaves!,
            job.SalaryInfo?.ToJobSalaryInfoDto(),
            job.EmploymentOptions!.Select(x => x.Id).ToList(),
            job.JobContractTypes!.Select(x => x.Id).ToList(),
            job.IsPublic,
            claimIdsForCurrentUser
            );
        
        return new GetJobManagementDtoResult(jobManagementDto);
    }
}
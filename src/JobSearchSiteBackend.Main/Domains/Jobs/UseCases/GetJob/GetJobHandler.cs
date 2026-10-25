using Ardalis.Result;
using AutoMapper;
using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Main.Domains.Companies.Persistence;
using JobSearchSiteBackend.Main.Domains.Jobs.Dtos;
using JobSearchSiteBackend.Main.Domains.Locations;
using JobSearchSiteBackend.Main.Persistence;
using JobSearchSiteBackend.Main.Services.FileStorage;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Main.Domains.Jobs.UseCases.GetJob;

public class GetJobHandler(
    MainDataContext context,
    IJobPageVisitCacheRepository cacheRepo,
    IFileStorageService fileStorageService) : IRequestHandler<GetJobQuery, Result<GetJobResult>>
{
    public async Task<Result<GetJobResult>> Handle(GetJobQuery query,
        CancellationToken cancellationToken = default)
    {
        var fetchedJob = await context.Jobs
            .AsNoTracking()
            .Where(j => j.Id == query.Id)
            .Include(j => j.Company)
            .ThenInclude(c => c!.CompanyAvatars)
            .Include(job => job.SalaryInfo)
            .Include(job => job.EmploymentOptions)
            .Include(job => job.JobContractTypes)
            .Include(job => job.Locations)
            .SingleOrDefaultAsync(cancellationToken);

        if (fetchedJob is null)
            return Result.NotFound();
        
        if (!fetchedJob.IsPublic)
            return Result.Forbidden();

        await cacheRepo.IncrementJobVisitsCounterAsync(fetchedJob.CompanyId.ToString(), fetchedJob.Id.ToString());
        
        var avatar = fetchedJob.Company?.CompanyAvatars?.GetLatestAvailableAvatar();

        string? avatarLink = null;

        if (avatar is not null)
        {
            avatarLink = await fileStorageService.GetDownloadUrlAsync(FileStorageBucketName.CompanyAvatars, 
                avatar.GuidIdentifier, avatar.Extension, cancellationToken);
        }
        
        var jobDetailedDto = new JobDetailedDto(
            fetchedJob.Id,
            fetchedJob.CompanyId,
            avatarLink,
            fetchedJob.Company!.Name,
            fetchedJob.Company.Description,
            fetchedJob.Locations!.Select(l => l.ToLocationDto()).ToList(),
            fetchedJob.CategoryId,
            fetchedJob.Title,
            fetchedJob.Description,
            fetchedJob.DateTimePublishedUtc,
            fetchedJob.DateTimeExpiringUtc,
            fetchedJob.Responsibilities!,
            fetchedJob.Requirements!,
            fetchedJob.NiceToHaves!,
            fetchedJob.SalaryInfo?.ToJobSalaryInfoDto(),
            fetchedJob.EmploymentOptions!.Select(eo => eo.Id).ToList(),
            fetchedJob.JobContractTypes!.Select(ct => ct.Id).ToList());
        
        return new GetJobResult(jobDetailedDto);
    }
}
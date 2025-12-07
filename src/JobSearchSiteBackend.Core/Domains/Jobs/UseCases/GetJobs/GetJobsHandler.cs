using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Companies.Persistence;
using JobSearchSiteBackend.Core.Domains.Jobs.Dtos;
using JobSearchSiteBackend.Core.Domains.Jobs.Search;
using JobSearchSiteBackend.Core.Domains.Locations;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.FileStorage;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetJobs;

public class GetJobsHandler(
    IJobSearchRepository jobSearchRepository,
    MainDataContext context,
    IFileStorageService fileStorageService,
    ICurrentAccountService currentAccountService) : IRequestHandler<GetJobsQuery, Result<GetJobsResult>>
{
    public async Task<Result<GetJobsResult>> Handle(GetJobsQuery query,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetId();
        
        var dbIncludeQuery = context.Jobs
            .AsNoTracking()
            .Include(j => j.Locations)
            .Include(j => j.JobContractTypes)
            .Include(j => j.EmploymentOptions)
            .Include(j => j.JobFolder)
            .ThenInclude(jf => jf!.Company)
            .ThenInclude(c => c!.CompanyAvatars);

        IQueryable<Job> dbQuery = dbIncludeQuery
            .Where(job => job.DateTimeExpiringUtc > DateTime.UtcNow)
            .Where(job => job.IsPublic);

        List<long>? hitIds = null;
        
        if (!string.IsNullOrEmpty(query.Query))
        {
            var hitIdsCollection = await jobSearchRepository
                .SearchFromCountriesAndCategoriesAsync(query.CountryIds ?? [], query.CategoryIds ?? [],
                    query.Query, cancellationToken);

            hitIds = hitIdsCollection.ToList();

            dbQuery = dbQuery
                .Where(job => hitIds.Contains(job.Id));
        }

        if (query.MustHaveSalaryRecord is not null && query.MustHaveSalaryRecord.Value)
            dbQuery = dbQuery.Where(job => job.SalaryInfo != null);

        if (query.EmploymentTypeIds is not null)
            dbQuery = dbQuery.Where(job => job.EmploymentOptions!.Any(x => query.EmploymentTypeIds.Contains(x.Id)));

        if (query.CategoryIds is not null && query.CategoryIds.Count != 0)
            dbQuery = dbQuery.Where(job => query.CategoryIds.Contains(job.CategoryId));

        if (query.CountryIds is not null && query.CountryIds.Count != 0)
            dbQuery = dbQuery.Where(job => query.CountryIds.Contains(job.JobFolder!.Company!.CountryId));

        if (query.ContractTypeIds is not null && query.ContractTypeIds.Count != 0)
            dbQuery = dbQuery.Where(job => job.JobContractTypes!.Any(jct => query.ContractTypeIds.Contains(jct.Id)));

        var count = await dbQuery.CountAsync(cancellationToken);

        List<(Job, bool)> jobsWithIsBookmarked = [];

        if (string.IsNullOrEmpty(query.Query))
        {
            dbQuery = dbQuery
                .OrderByDescending(job => job.DateTimePublishedUtc)
                .Skip((query.Page - 1) * query.Size)
                .Take(query.Size);
        }

        if (currentUserId is null)
        {
            var jobs = await dbQuery.ToListAsync(cancellationToken);

            jobsWithIsBookmarked = jobs.Select(j => (j, false)).ToList();
        }
        else
        {
            jobsWithIsBookmarked = await dbQuery
                .Select(j => new ValueTuple<Job, bool>
                (
                    j,
                    j.UserJobBookmarks!.Any(ujb => ujb.UserId == currentUserId)
                ))
                .ToListAsync(cancellationToken);
        }
        
        if (jobsWithIsBookmarked.Count == 0)
        {
            return Result.NotFound();
        }
        
        if (!string.IsNullOrEmpty(query.Query))
        {
            if (hitIds is null)
                throw new NullReferenceException();
            
            jobsWithIsBookmarked = jobsWithIsBookmarked
                .OrderBy(jwib => hitIds.IndexOf(jwib.Item1.Id))
                .Skip((query.Page - 1) * query.Size)
                .Take(query.Size)
                .ToList();
        }

        List<JobCardDto> jobCardDtos = [];

        foreach (var (job, isBookmarked) in jobsWithIsBookmarked)
        {
            var avatar = job.JobFolder?.Company?.CompanyAvatars?.GetLatestAvailableAvatar();

            string? avatarLink = null;

            if (avatar is not null)
            {
                avatarLink = await fileStorageService.GetDownloadUrlAsync(FileStorageBucketName.CompanyAvatars, 
                    avatar.GuidIdentifier, avatar.Extension, cancellationToken);
            }
            
            var jobCardDto = new JobCardDto(job.Id, job.JobFolder!.CompanyId, avatarLink, job.JobFolder!.Company!.Name,
                job.Locations!.Select(l => l.ToLocationDto()).ToList(),
                job.Title, job.DateTimePublishedUtc, job.DateTimeExpiringUtc, job.SalaryInfo?.ToJobSalaryInfoDto(),
                job.EmploymentOptions!.Select(eo => eo.Id).ToList(),
                job.JobContractTypes!.Select(ct => ct.Id).ToList(), isBookmarked);
            
            jobCardDtos.Add(jobCardDto);
        }

        var paginationResponse = new PaginationResponse(query.Page, query.Size, count);

        var response = new GetJobsResult(jobCardDtos, paginationResponse);

        return Result.Success(response);
    }
}
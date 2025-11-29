using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Jobs.Dtos;
using JobSearchSiteBackend.Core.Domains.Jobs.Search;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;
using AutoMapper;
using JobSearchSiteBackend.Core.Domains.Companies.Persistence;
using JobSearchSiteBackend.Core.Domains.Locations.Dtos;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.FileStorage;

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
            .Include(j => j.JobContractTypes)
            .Include(j => j.EmploymentOptions)
            .Include(j => j.JobFolder)
            .ThenInclude(jf => jf!.Company)
            .ThenInclude(c => c!.CompanyAvatars);

        var dbQuery = dbIncludeQuery
            .Where(job => job.DateTimeExpiringUtc > DateTime.UtcNow)
            .Where(job => job.IsPublic);

        if (!string.IsNullOrEmpty(query.Query))
        {
            var hitIds = await jobSearchRepository
                .SearchFromCountriesAndCategoriesAsync(query.CountryIds ?? [], query.CategoryIds ?? [],
                    query.Query, cancellationToken);

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

        var paginatedDbQuery = dbQuery
            .OrderByDescending(job => job.DateTimePublishedUtc)
            .Skip((query.Page - 1) * query.Size)
            .Take(query.Size);

        if (currentUserId is null)
        {
            var jobs = await paginatedDbQuery.ToListAsync(cancellationToken);

            jobsWithIsBookmarked = jobs.Select(j => (j, false)).ToList();
        }
        else
        {
            jobsWithIsBookmarked = await paginatedDbQuery
                .Select(j => new ValueTuple<Job, bool>
                (
                    j,
                    j.UsersWhoBookmarked!.Any(u => u.Id == currentUserId)
                ))
                .ToListAsync(cancellationToken);
        }

        List<JobCardDto> jobCardDtos = [];

        foreach (var (job, isBookmarked) in jobsWithIsBookmarked)
        {
            var avatar = job.JobFolder?.Company?.CompanyAvatars?.GetLatestAvailableAvatar();

            string? avatarLink = null;

            if (avatar is not null)
            {
                avatarLink = await fileStorageService.GetDownloadUrlAsync(FileStorageBucketName.UserAvatars, 
                    avatar.GuidIdentifier, avatar.Extension, cancellationToken);
            }
            
            // todo work on locations
            
            var jobCardDto = new JobCardDto(job.Id, job.JobFolder!.CompanyId, avatarLink,
                job.JobFolder!.Company!.Name, [], job.Title, job.DateTimePublishedUtc,
                job.DateTimeExpiringUtc, job.SalaryInfo?.ToJobSalaryInfoDto(),
                job.EmploymentOptions!.Select(eo => eo.Id).ToList(),
                job.JobContractTypes!.Select(ct => ct.Id).ToList(), isBookmarked);
            
            jobCardDtos.Add(jobCardDto);
        }

        var paginationResponse = new PaginationResponse(query.Page, query.Size, count);

        var response = new GetJobsResult(jobCardDtos, paginationResponse);

        return response;
    }
}
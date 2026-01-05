using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Companies.Persistence;
using JobSearchSiteBackend.Core.Domains.Jobs.Dtos;
using JobSearchSiteBackend.Core.Domains.Jobs.Search;
using JobSearchSiteBackend.Core.Domains.Locations.Dtos;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.FileStorage;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyJobs;

public class GetCompanyJobsHandler(
    IJobSearchRepository jobSearchRepository,
    IFileStorageService fileStorageService,
    MainDataContext context,
    ICurrentAccountService currentAccountService) : IRequestHandler<GetCompanyJobsQuery, Result<GetCompanyJobsResult>>
{
    private record JobItem(
        long Id,
        long CompanyId,
        string CompanyName,
        IEnumerable<LocationDto> Locations,
        string Title,
        DateTime DateTimePublishedUtc,
        DateTime DateTimeExpiringUtc,
        JobSalaryInfoDto? SalaryInfoDto,
        IEnumerable<long> JobContractTypeIds,
        IEnumerable<long> EmploymentOptionIds,
        IEnumerable<CompanyAvatar> CompanyAvatars,
        bool IsBookmarked);

    public async Task<Result<GetCompanyJobsResult>> Handle(GetCompanyJobsQuery query,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetId();

        var dbQuery = context.Jobs
            .Where(job => job.CompanyId == query.CompanyId)
            .Where(job => job.DateTimeExpiringUtc > DateTime.UtcNow)
            .Where(job => job.IsPublic)
            .Where(job => !job.IsDeleted);

        if (!string.IsNullOrEmpty(query.Query))
        {
            var hitIds = await jobSearchRepository
                .SearchFromCompanyAsync(query.CompanyId, query.Query, cancellationToken);

            dbQuery = dbQuery
                .Where(job => hitIds.Contains(job.Id));
        }

        if (query.MustHaveSalaryRecord is not null && query.MustHaveSalaryRecord.Value)
            dbQuery = dbQuery.Where(job => job.SalaryInfo != null);

        if (query.EmploymentTypeIds is not null)
            dbQuery = dbQuery.Where(job => job.EmploymentOptions!.Any(x => query.EmploymentTypeIds.Contains(x.Id)));

        if (query.CategoryIds is not null && query.CategoryIds.Count != 0)
            dbQuery = dbQuery.Where(job => query.CategoryIds.Contains(job.CategoryId));

        if (query.ContractTypeIds is not null && query.ContractTypeIds.Count != 0)
            dbQuery = dbQuery.Where(job => job.JobContractTypes!.Any(jct => query.ContractTypeIds.Contains(jct.Id)));

        var count = await dbQuery.CountAsync(cancellationToken);

        List<JobItem> jobItems = [];

        var paginatedDbQuery = dbQuery
            .OrderByDescending(job => job.DateTimePublishedUtc)
            .Skip((query.Page - 1) * query.Size)
            .Take(query.Size);

        if (currentUserId is null)
        {
            jobItems = await paginatedDbQuery
                .Select(j => new JobItem(
                        j.Id,
                        j.CompanyId,
                        j.Company!.Name,
                        j.Locations!
                            .Select(l => new LocationDto(l.Id, l.CountryId, l.FullName, l.DescriptionPl, l.Code)),
                        j.Title, j.DateTimePublishedUtc, j.DateTimeExpiringUtc,
                        j.SalaryInfo != null ? new JobSalaryInfoDto(j.SalaryInfo!.Minimum, j.SalaryInfo.Maximum,
                            j.SalaryInfo.CurrencyId, j.SalaryInfo.UnitOfTime, j.SalaryInfo.IsAfterTaxes) : null,
                        j.JobContractTypes!.Select(jct => jct.Id),
                        j.EmploymentOptions!.Select(eo => eo.Id),
                        j.Company!.CompanyAvatars!,
                        false
                    )
                )
                .ToListAsync(cancellationToken);
        }
        else
        {
            jobItems = await paginatedDbQuery
                .Select(j => new JobItem(
                        j.Id,
                        j.CompanyId,
                        j.Company!.Name,
                        j.Locations!
                            .Select(l => new LocationDto(l.Id, l.CountryId, l.FullName, l.DescriptionPl, l.Code)),
                        j.Title, j.DateTimePublishedUtc, j.DateTimeExpiringUtc,
                        j.SalaryInfo != null
                            ? new JobSalaryInfoDto(j.SalaryInfo.Minimum, j.SalaryInfo.Maximum,
                                j.SalaryInfo.CurrencyId, j.SalaryInfo.UnitOfTime, j.SalaryInfo.IsAfterTaxes)
                            : null,
                        j.JobContractTypes!.Select(jct => jct.Id),
                        j.EmploymentOptions!.Select(eo => eo.Id),
                        j.Company!.CompanyAvatars!,
                        j.UserJobBookmarks!.Any(u => u.UserId == currentUserId)
                    )
                )
                .ToListAsync(cancellationToken);
        }

        if (jobItems.Count == 0)
        {
            return Result.NotFound();
        }

        var avatar = jobItems.First().CompanyAvatars.ToList().GetLatestAvailableAvatar();

        string? avatarLink = null;

        if (avatar is not null)
        {
            avatarLink = await fileStorageService.GetDownloadUrlAsync(FileStorageBucketName.CompanyAvatars,
                avatar.GuidIdentifier, avatar.Extension, cancellationToken);
        }

        var jobCardDtos = jobItems
            .Select(jobItem => new JobCardDto(jobItem.Id, jobItem.CompanyId, avatarLink,
                jobItem.CompanyName, jobItem.Locations.ToList(), jobItem.Title, jobItem.DateTimePublishedUtc,
                jobItem.DateTimeExpiringUtc, jobItem.SalaryInfoDto, jobItem.EmploymentOptionIds.ToList(),
                jobItem.JobContractTypeIds.ToList(), jobItem.IsBookmarked))
            .ToList();

        var paginationResponse = new PaginationResponse(query.Page, query.Size, count);

        var response = new GetCompanyJobsResult(jobCardDtos, paginationResponse);

        return Result.Success(response);
    }
}
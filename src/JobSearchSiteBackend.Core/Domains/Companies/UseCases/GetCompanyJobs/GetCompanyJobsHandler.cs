using Ardalis.Result;
using AutoMapper;
using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Jobs.Dtos;
using JobSearchSiteBackend.Core.Domains.Jobs.Search;
using JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetJobs;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.FileStorage;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyJobs;

public class GetCompanyJobsHandler(
    IJobSearchRepository jobSearchRepository,
    IFileStorageService fileStorageService,
    MainDataContext context,
    IMapper mapper) : IRequestHandler<GetCompanyJobsQuery, Result<GetCompanyJobsResult>>
{
    public async Task<Result<GetCompanyJobsResult>> Handle(GetCompanyJobsQuery query,
        CancellationToken cancellationToken = default)
    {
        var hitIds = await jobSearchRepository
            .SearchFromCompanyAsync(query.CompanyId, query.Query, cancellationToken);

        var dbQuery = context.Jobs
            .Include(j => j.EmploymentOptions)
            .Include(j => j.JobFolder)
            .AsNoTracking()
            .Where(job => job.JobFolder!.CompanyId == query.CompanyId)
            .Where(job => job.DateTimeExpiringUtc > DateTime.UtcNow)
            .Where(job => job.IsPublic)
            .Where(job => hitIds.Contains(job.Id));

        if (query.MustHaveSalaryRecord is not null && query.MustHaveSalaryRecord.Value)
            dbQuery = dbQuery.Where(job => job.SalaryInfo != null);

        if (query.EmploymentTypeIds is not null)
            dbQuery = dbQuery.Where(job => job.EmploymentOptions!.Any(x => query.EmploymentTypeIds.Contains(x.Id)));

        if (query.CategoryIds is not null && query.CategoryIds.Count != 0)
            dbQuery = dbQuery.Where(job => query.CategoryIds.Contains(job.CategoryId));

        if (query.ContractTypeIds is not null && query.ContractTypeIds.Count != 0)
            dbQuery = dbQuery.Where(job => job.JobContractTypes!.Any(jct => query.ContractTypeIds.Contains(jct.Id)));

        var count = await dbQuery.CountAsync(cancellationToken);

        var jobs = await dbQuery
            .OrderByDescending(job => job.DateTimePublishedUtc)
            .Skip((query.Page - 1) * query.Size)
            .Take(query.Size)
            .ToListAsync(cancellationToken);
        
        var lastAvatar = await context.CompanyAvatars
            .Where(a => a.CompanyId == query.CompanyId)
            .Where(a => !a.IsDeleted && a.IsUploadedSuccessfully)
            .OrderBy(a => a.DateTimeUpdatedUtc)
            .LastOrDefaultAsync(cancellationToken);

        string? companyLogoLink = null;
            
        if (lastAvatar is not null)
        {
            companyLogoLink = await fileStorageService.GetDownloadUrlAsync(FileStorageBucketName.CompanyAvatars,
                lastAvatar.GuidIdentifier, lastAvatar.Extension, cancellationToken);
        }

        var paginationResponse = new PaginationResponse(query.Page, query.Size, count);

        var jobCardDtos = jobs
            .Select((x, i) =>
                mapper.Map<JobCardDto>(x, opt => { opt.State = companyLogoLink; }))
            .ToList();

        var response = new GetCompanyJobsResult(jobCardDtos, paginationResponse);

        return response;
    }
}
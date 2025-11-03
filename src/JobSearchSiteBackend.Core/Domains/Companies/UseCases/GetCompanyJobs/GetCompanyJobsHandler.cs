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
    IMapper mapper) : IRequestHandler<GetCompanyJobsRequest, Result<GetCompanyJobsResponse>>
{
    public async Task<Result<GetCompanyJobsResponse>> Handle(GetCompanyJobsRequest request,
        CancellationToken cancellationToken = default)
    {
        var hitIds = await jobSearchRepository
            .SearchFromCompanyAsync(request.CompanyId, request.Query, cancellationToken);

        var query = context.Jobs
            .Include(j => j.EmploymentTypes)
            .Include(j => j.JobFolder)
            .AsNoTracking()
            .Where(job => job.JobFolder!.CompanyId == request.CompanyId)
            .Where(job => job.DateTimeExpiringUtc > DateTime.UtcNow)
            .Where(job => job.IsPublic)
            .Where(job => hitIds.Contains(job.Id));

        if (request.MustHaveSalaryRecord is not null && request.MustHaveSalaryRecord.Value)
            query = query.Where(job => job.SalaryInfo != null);

        if (request.EmploymentTypeIds is not null)
            query = query.Where(job => job.EmploymentTypes!.Any(x => request.EmploymentTypeIds.Contains(x.Id)));

        if (request.CategoryIds is not null && request.CategoryIds.Count != 0)
            query = query.Where(job => request.CategoryIds.Contains(job.CategoryId));

        if (request.ContractTypeIds is not null && request.ContractTypeIds.Count != 0)
            query = query.Where(job => job.JobContractTypes!.Any(jct => request.ContractTypeIds.Contains(jct.Id)));

        var count = await query.CountAsync(cancellationToken);

        var jobs = await query
            .OrderByDescending(job => job.DateTimePublishedUtc)
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .ToListAsync(cancellationToken);
        
        var lastAvatar = await context.CompanyAvatars
            .Where(a => a.CompanyId == request.CompanyId)
            .Where(a => !a.IsDeleted && a.IsUploadedSuccessfully)
            .OrderBy(a => a.DateTimeUpdatedUtc)
            .LastOrDefaultAsync(cancellationToken);

        string? companyLogoLink = null;
            
        if (lastAvatar is not null)
        {
            companyLogoLink = await fileStorageService.GetDownloadUrlAsync(FileStorageBucketName.CompanyAvatars,
                lastAvatar.GuidIdentifier, lastAvatar.Extension, cancellationToken);
        }

        var paginationResponse = new PaginationResponse(request.Page, request.Size, count);

        var jobCardDtos = jobs
            .Select((x, i) =>
                mapper.Map<JobCardDto>(x, opt => { opt.State = companyLogoLink; }))
            .ToList();

        var response = new GetCompanyJobsResponse(jobCardDtos, paginationResponse);

        return response;
    }
}
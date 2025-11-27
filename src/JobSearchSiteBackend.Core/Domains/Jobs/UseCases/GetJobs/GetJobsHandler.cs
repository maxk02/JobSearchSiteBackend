using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Jobs.Dtos;
using JobSearchSiteBackend.Core.Domains.Jobs.Search;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;
using AutoMapper;
using JobSearchSiteBackend.Core.Persistence;

namespace JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetJobs;

public class GetJobsHandler(
    IJobSearchRepository jobSearchRepository,
    MainDataContext context,
    IMapper mapper) : IRequestHandler<GetJobsQuery, Result<GetJobsResult>>
{
    public async Task<Result<GetJobsResult>> Handle(GetJobsQuery query,
        CancellationToken cancellationToken = default)
    {
        var dbQuery = context.Jobs
            .Include(j => j.EmploymentOptions)
            .AsNoTracking()
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

        var jobs = await dbQuery
            .OrderByDescending(job => job.DateTimePublishedUtc)
            .Skip((query.Page - 1) * query.Size)
            .Take(query.Size)
            .ToListAsync(cancellationToken);
        
        // list with logo links
        var companyLogoLinks = jobs.Select(x => x.Id).Select(x => "").ToList(); //todo

        var paginationResponse = new PaginationResponse(query.Page, query.Size, count);

        var jobCardDtos = jobs
            .Select((x, i) =>
                mapper.Map<JobCardDto>(x, opt => { opt.State = companyLogoLinks[i]; }))
            .ToList();

        var response = new GetJobsResult(jobCardDtos, paginationResponse);

        return response;
    }
}
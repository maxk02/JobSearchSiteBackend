using Core.Domains._Shared.Pagination;
using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.Jobs.Dtos;
using Core.Domains.Jobs.Search;
using Core.Persistence.EfCore;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;
using AutoMapper;

namespace Core.Domains.Jobs.UseCases.GetJobs;

public class GetJobsHandler(
    IJobSearchRepository jobSearchRepository,
    MainDataContext context,
    IMapper mapper) : IRequestHandler<GetJobsRequest, Result<GetJobsResponse>>
{
    public async Task<Result<GetJobsResponse>> Handle(GetJobsRequest request,
        CancellationToken cancellationToken = default)
    {
        var hitIds = await jobSearchRepository
            .SearchFromCountriesAndCategoriesAsync(request.CountryIds ?? [], request.CategoryIds ?? [],
                request.Query, cancellationToken);

        var query = context.Jobs
            .Include(j => j.EmploymentTypes)
            .AsNoTracking()
            .Where(job => job.DateTimeExpiringUtc > DateTime.UtcNow)
            .Where(job => job.IsPublic)
            .Where(job => hitIds.Contains(job.Id));

        if (request.MustHaveSalaryRecord is not null && request.MustHaveSalaryRecord.Value)
            query = query.Where(job => job.SalaryInfo != null);

        if (request.EmploymentTypeIds is not null)
            query = query.Where(job => job.EmploymentTypes!.Any(x => request.EmploymentTypeIds.Contains(x.Id)));

        if (request.CategoryIds is not null && request.CategoryIds.Count != 0)
            query = query.Where(job => request.CategoryIds.Contains(job.CategoryId));

        if (request.CountryIds is not null && request.CountryIds.Count != 0)
            query = query.Where(job => request.CountryIds.Contains(job.JobFolder!.Company!.CountryId));

        if (request.ContractTypeIds is not null && request.ContractTypeIds.Count != 0)
            query = query.Where(job => job.JobContractTypes!.Any(jct => request.ContractTypeIds.Contains(jct.Id)));

        var count = await query.CountAsync(cancellationToken);

        var jobs = await query
            .OrderByDescending(job => job.DateTimePublishedUtc)
            .Skip((request.PaginationSpec.PageNumber - 1) * request.PaginationSpec.PageSize)
            .Take(request.PaginationSpec.PageSize)
            .ToListAsync(cancellationToken);
        
        // list with logo links
        var companyLogoLinks = jobs.Select(x => x.Id).Select(x => "").ToList();

        var paginationResponse = new PaginationResponse(count, request.PaginationSpec.PageNumber,
            request.PaginationSpec.PageSize);

        var jobCardDtos = jobs
            .Select((x, i) =>
                mapper.Map<JobCardDto>(x, opt => { opt.State = companyLogoLinks[i]; }))
            .ToList();

        var response = new GetJobsResponse(jobCardDtos, paginationResponse);

        return response;
    }
}
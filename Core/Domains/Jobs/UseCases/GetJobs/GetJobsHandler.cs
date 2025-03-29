using Core.Domains._Shared.Pagination;
using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.Jobs.Dtos;
using Core.Domains.Jobs.Search;
using Core.Persistence.EfCore;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;

namespace Core.Domains.Jobs.UseCases.GetJobs;

public class GetJobsHandler(
    IJobSearchRepository jobSearchRepository,
    MainDataContext context) : IRequestHandler<GetJobsRequest, Result<GetJobsResponse>>
{
    public async Task<Result<GetJobsResponse>> Handle(GetJobsRequest request,
        CancellationToken cancellationToken = default)
    {
        var hitIds = await jobSearchRepository
            .SearchFromCountriesAndCategoriesAsync(request.CountryIds ?? [], request.CategoryIds ?? [],
                request.Query, cancellationToken);

        var query = context.Jobs
            .AsNoTracking()
            .Where(job => job.DateTimeExpiringUtc > DateTime.UtcNow)
            .Where(job => job.IsPublic)
            .Where(job => hitIds.Contains(job.Id));

        if (request.MustHaveSalaryRecord is not null && request.MustHaveSalaryRecord.Value)
            query = query.Where(job => job.SalaryRecord != null);

        if (request.EmploymentTypeRecord is not null)
            query = query.Where(job => job.EmploymentTypeRecord == request.EmploymentTypeRecord);

        if (request.CategoryIds is not null && request.CategoryIds.Count != 0)
            query = query.Where(job => request.CategoryIds.Contains(job.CategoryId));

        if (request.CountryIds is not null && request.CountryIds.Count != 0)
            query = query.Where(job => request.CountryIds.Contains(job.JobFolder!.Company!.CountryId));

        if (request.ContractTypeIds is not null && request.ContractTypeIds.Count != 0)
            query = query.Where(job => job.JobContractTypes!.Any(jct => request.ContractTypeIds.Contains(jct.Id)));

        var count = await query.CountAsync(cancellationToken);

        var jobInfoDtos = await query
            .OrderByDescending(job => job.DateTimePublishedUtc)
            .Skip((request.PaginationSpec.PageNumber - 1) * request.PaginationSpec.PageSize)
            .Take(request.PaginationSpec.PageSize)
            .Select(x => new JobInfoDto(x.Id, x.JobFolder!.CompanyId, x.CategoryId, x.Title,
                x.DateTimePublishedUtc, x.DateTimeExpiringUtc, x.SalaryRecord, x.EmploymentTypeRecord))
            .ToListAsync(cancellationToken);

        var paginationResponse = new PaginationResponse(count, request.PaginationSpec.PageNumber,
            request.PaginationSpec.PageSize);

        var response = new GetJobsResponse(jobInfoDtos, paginationResponse);

        return response;
    }
}
using Core.Domains._Shared.Pagination;
using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.Jobs.Dtos;
using Core.Domains.Jobs.Search;
using Core.Persistence.EfCore;
using Microsoft.EntityFrameworkCore;
using Shared.Result;

namespace Core.Domains.Jobs.UseCases.GetJobs;

public class GetJobsHandler(
    IJobSearchRepository jobSearchRepository,
    MainDataContext context) : IRequestHandler<GetJobsRequest, Result<GetJobsResponse>>
{
    public async Task<Result<GetJobsResponse>> Handle(GetJobsRequest request,
        CancellationToken cancellationToken = default)
    {
        var hitIds =
            await jobSearchRepository.SearchByCountryIdAsync(request.CountryId, request.Query, cancellationToken);

        var query = context.Jobs
            .AsNoTracking()
            .Where(job => job.IsPublic)
            .Where(job => job.JobFolder!.Company!.CountryId == request.CountryId)
            .Where(job => hitIds.Contains(job.Id));

        if (request.MustHaveSalaryRecord is not null && request.MustHaveSalaryRecord.Value == true)
            query = query.Where(job => job.SalaryRecord != null);

        if (request.EmploymentTypeRecord is not null)
            query = query.Where(job => job.EmploymentTypeRecord == request.EmploymentTypeRecord);

        if (request.CategoryIds is not null && request.CategoryIds.Count != 0)
            query = query.Where(job => request.CategoryIds.Contains(job.CategoryId));

        if (request.ContractTypeIds is not null && request.ContractTypeIds.Count != 0)
            query = query.Where(job => job.JobContractTypes!.Any(jct => request.ContractTypeIds.Contains(jct.Id)));

        var count = await query.CountAsync(cancellationToken);

        var jobInfocardDtos = await query
            .Skip((request.PaginationSpec.PageNumber - 1) * request.PaginationSpec.PageSize)
            .Take(request.PaginationSpec.PageSize)
            .Select(x => new JobInfocardDto(x.Id, x.JobFolder!.CompanyId, x.CategoryId, x.Title,
                x.DateTimePublishedUtc, x.DateTimeExpiringUtc, x.SalaryRecord, x.EmploymentTypeRecord))
            .ToListAsync(cancellationToken);

        var paginationResponse = new PaginationResponse(count, request.PaginationSpec.PageNumber,
            request.PaginationSpec.PageSize);

        var response = new GetJobsResponse(jobInfocardDtos, paginationResponse);

        return response;
    }
}
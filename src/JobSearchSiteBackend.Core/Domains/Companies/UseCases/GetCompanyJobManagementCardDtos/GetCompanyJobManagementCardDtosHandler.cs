using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.CompanyClaims;
using JobSearchSiteBackend.Core.Domains.Jobs.Dtos;
using JobSearchSiteBackend.Core.Domains.Locations.Dtos;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyJobManagementCardDtos;

public class GetCompanyJobManagementCardDtosHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<GetCompanyJobManagementCardDtosQuery, Result<GetCompanyJobManagementCardDtosResult>>
{
    public async Task<Result<GetCompanyJobManagementCardDtosResult>> Handle(GetCompanyJobManagementCardDtosQuery query,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var hasPermissionInRequestedCompany =
            await context.UserCompanyClaims
                .Where(ucc => ucc.CompanyId == query.CompanyId
                    && ucc.UserId == currentUserId
                    && ucc.ClaimId == CompanyClaim.CanReadJobs.Id)
                .AnyAsync(cancellationToken);

        if (!hasPermissionInRequestedCompany)
            return Result.Forbidden();

        var dbQuery = context.Jobs
            .Where(job => job.CompanyId == query.CompanyId)
            .Where(job => !job.IsDeleted);

        if (!string.IsNullOrEmpty(query.Query))
            dbQuery = dbQuery.Where(jobItem => jobItem.Title.ToLower().Contains(query.Query.ToLower()));
        
        if (query.MustHaveSalaryRecord is not null && query.MustHaveSalaryRecord.Value)
            dbQuery = dbQuery.Where(job => job.SalaryInfo != null);

        if (query.EmploymentTypeIds is not null && query.EmploymentTypeIds.Count != 0)
            dbQuery = dbQuery.Where(job => job.EmploymentOptions!.Any(x => query.EmploymentTypeIds.Contains(x.Id)));

        if (query.CategoryIds is not null && query.CategoryIds.Count != 0)
            dbQuery = dbQuery.Where(job => query.CategoryIds.Contains(job.CategoryId));

        if (query.ContractTypeIds is not null && query.ContractTypeIds.Count != 0)
            dbQuery = dbQuery.Where(job => job.JobContractTypes!.Any(jct => query.ContractTypeIds.Contains(jct.Id)));
        
        var count = await dbQuery.CountAsync(cancellationToken);
        
        var jobManagementCardDtos = await dbQuery
            .Select(job => new JobManagementCardDto(
                job.Id,
                job.Locations!.Select(l => new LocationDto(l.Id,
                    l.CountryId, l.FullName, l.DescriptionPl, l.Code)).ToList(),
                job.Title,
                job.DateTimePublishedUtc,
                job.DateTimeExpiringUtc,
                job.SalaryInfo != null ? new JobSalaryInfoDto(job.SalaryInfo.Minimum, job.SalaryInfo.Maximum,
                    job.SalaryInfo.CurrencyId, job.SalaryInfo.UnitOfTime, job.SalaryInfo.IsAfterTaxes) : null,
                job.EmploymentOptions!.Select(eo => eo.Id).ToList(),
                job.JobContractTypes!.Select(jct => jct.Id).ToList(),
                job.UserJobBookmarks!.Any(ujb => ujb.UserId == currentUserId),
                job.IsPublic))
            .ToListAsync(cancellationToken);
        
        var paginationResponse = new PaginationResponse(query.Page, query.Size, count);
        
        var response = new GetCompanyJobManagementCardDtosResult(jobManagementCardDtos, paginationResponse);
        
        return Result.Success(response);
    }
}
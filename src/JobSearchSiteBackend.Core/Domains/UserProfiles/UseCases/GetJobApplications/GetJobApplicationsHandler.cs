using Ardalis.Result;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.JobApplications.Dtos;
using JobSearchSiteBackend.Core.Domains.Jobs;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.GetJobApplications;

public class GetJobApplicationsHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context)
    : IRequestHandler<GetJobApplicationsQuery, Result<GetJobApplicationsResult>>
{
    public async Task<Result<GetJobApplicationsResult>> Handle(GetJobApplicationsQuery query,
        CancellationToken cancellationToken = default)
    {
        var currentAccountId = currentAccountService.GetIdOrThrow();

        var dbQuery = context.JobApplications
            .Where(jobApplication => jobApplication.UserId == currentAccountId);
        
        var count = await dbQuery.CountAsync(cancellationToken);

        var jobApplicationObjects = await dbQuery
            .OrderByDescending(ja => ja.DateTimeCreatedUtc)
            .Skip((query.Page - 1) * query.Size)
            .Take(query.Size)
            .Select(ja => new
            {
                Id = ja.Id,
                CompanyId = ja.Job!.JobFolder!.CompanyId,
                CompanyName = ja.Job!.JobFolder!.Company!.Name,
                JobId = ja.JobId,
                JobTitle = ja.Job!.Title,
                DateTimePublishedUtc = ja.Job!.DateTimePublishedUtc,
                JobSalaryInfo = ja.Job!.SalaryInfo,
                EmploymentTypeIds = ja.Job!.EmploymentOptions!.Select(eo => eo.Id),
                DateTimeAppliedUtc = ja.DateTimeCreatedUtc,
                Status = ja.Status
            })
            .ToListAsync(cancellationToken);

        var jobApplicationDtos = jobApplicationObjects
            .Select(jao => new JobApplicationInUserProfileDto(
                jao.Id,
                jao.CompanyId,
                jao.CompanyName,
                jao.JobId,
                jao.JobTitle,
                jao.DateTimePublishedUtc,
                jao.JobSalaryInfo?.ToJobSalaryInfoDto(),
                jao.EmploymentTypeIds.ToList(),
                jao.DateTimeAppliedUtc,
                jao.Status
                ))
            .ToList();
        
        var paginationResponse = new PaginationResponse(query.Page,
            query.Size, count);

        var response = new GetJobApplicationsResult(jobApplicationDtos, paginationResponse);

        return response;
    }
}
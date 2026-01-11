using Ardalis.Result;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Companies.Persistence;
using JobSearchSiteBackend.Core.Domains.JobApplications.Dtos;
using JobSearchSiteBackend.Core.Domains.Jobs;
using JobSearchSiteBackend.Core.Domains.Locations;
using JobSearchSiteBackend.Core.Domains.PersonalFiles;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.FileStorage;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.GetJobApplications;

public class GetJobApplicationsHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context,
    IFileStorageService fileStorageService)
    : IRequestHandler<GetJobApplicationsQuery, Result<GetJobApplicationsResult>>
{
    public async Task<Result<GetJobApplicationsResult>> Handle(GetJobApplicationsQuery query,
        CancellationToken cancellationToken = default)
    {
        var currentAccountId = currentAccountService.GetIdOrThrow();

        var dbQuery = context.JobApplications
            .Where(jobApplication => jobApplication.UserId == currentAccountId);
        
        var count = await dbQuery.CountAsync(cancellationToken);

        var jobApplicationItems = await dbQuery
            .OrderByDescending(ja => ja.DateTimeCreatedUtc)
            .Skip((query.Page - 1) * query.Size)
            .Take(query.Size)
            .Select(ja => new
            {
                Id = ja.Id,
                CompanyId = ja.Job!.CompanyId,
                CompanyName = ja.Job!.Company!.Name,
                CompanyAvatars = ja.Job!.Company!.CompanyAvatars!,
                JobId = ja.JobId,
                JobTitle = ja.Job!.Title,
                DateTimePublishedUtc = ja.Job!.DateTimePublishedUtc,
                JobSalaryInfo = ja.Job!.SalaryInfo,
                Location = ja.Location!,
                EmploymentOptionIds = ja.Job!.EmploymentOptions!.Select(eo => eo.Id),
                ContractTypeIds = ja.Job!.JobContractTypes!.Select(ct => ct.Id),
                DateTimeAppliedUtc = ja.DateTimeCreatedUtc,
                PersonalFiles = ja.PersonalFiles!,
                Status = ja.Status
            })
            .ToListAsync(cancellationToken);

        if (jobApplicationItems.Count == 0)
            return Result.NotFound();

        List<JobApplicationInUserProfileDto> jobApplicationInUserProfileDtos = [];
        
        foreach (var jai in jobApplicationItems)
        {
            var avatar = jai.CompanyAvatars.ToList().GetLatestAvailableAvatar();

            string? avatarLink = null;

            if (avatar is not null)
            {
                avatarLink = await fileStorageService.GetDownloadUrlAsync(FileStorageBucketName.CompanyAvatars, 
                    avatar.GuidIdentifier, avatar.Extension, cancellationToken);
            }

            var jobApplicationDto = new JobApplicationInUserProfileDto(
                jai.Id,
                jai.CompanyId,
                jai.CompanyName,
                avatarLink,
                jai.JobId,
                jai.JobTitle,
                jai.DateTimePublishedUtc,
                jai.JobSalaryInfo?.ToJobSalaryInfoDto(),
                jai.Location.ToLocationDto(),
                jai.EmploymentOptionIds.ToList(),
                jai.ContractTypeIds.ToList(),
                jai.DateTimeAppliedUtc,
                jai.PersonalFiles.Select(pf => pf.ToPersonalFileInfoDto()).ToList(),
                (int)jai.Status
            );
            
            jobApplicationInUserProfileDtos.Add(jobApplicationDto);
        }
        
        var paginationResponse = new PaginationResponse(query.Page,
            query.Size, count);

        var result = new GetJobApplicationsResult(jobApplicationInUserProfileDtos, paginationResponse);

        return Result.Success(result);
    }
}
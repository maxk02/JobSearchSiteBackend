using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Companies.Persistence;
using JobSearchSiteBackend.Core.Domains.Jobs;
using JobSearchSiteBackend.Core.Domains.Jobs.Dtos;
using JobSearchSiteBackend.Core.Domains.Locations;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.FileStorage;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.GetBookmarkedJobs;

public class GetBookmarkedJobsHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context,
    IFileStorageService fileStorageService)
    : IRequestHandler<GetBookmarkedJobsQuery, Result<GetBookmarkedJobsResult>>
{
    public async Task<Result<GetBookmarkedJobsResult>> Handle(GetBookmarkedJobsQuery query,
        CancellationToken cancellationToken)
    {
        var currentAccountId = currentAccountService.GetIdOrThrow();

        var dbBasicQuery = context.UserJobBookmarks
            .Where(ujb => ujb.UserId == currentAccountId)
            .Where(ujb => ujb.Job!.DateTimeExpiringUtc > DateTime.UtcNow)
            .Where(ujb => ujb.Job!.IsPublic);
        
        var count = await dbBasicQuery.CountAsync(cancellationToken);
        
        var dbQuery = dbBasicQuery
            .OrderByDescending(ujb => ujb.DateTimeCreatedUtc)
            .Select(ujb => new
            {
                JobId = ujb.JobId,
                CompanyId = ujb.Job!.CompanyId,
                CompanyAvatars = ujb.Job!.Company!.CompanyAvatars,
                CompanyName = ujb.Job!.Company!.Name,
                Locations = ujb.Job!.Locations!,
                Title = ujb.Job!.Title,
                DateTimePublishedUtc = ujb.Job!.DateTimePublishedUtc,
                DateTimeExpiringUtc = ujb.Job!.DateTimeExpiringUtc,
                SalaryInfo = ujb.Job!.SalaryInfo,
                EmploymentOptionIds = ujb.Job!.EmploymentOptions!.Select(eo => eo.Id),
                ContractTypeIds = ujb.Job!.JobContractTypes!.Select(ct => ct.Id)
            });
        
        var jobObjects = await dbQuery
            .Skip((query.Page - 1) * query.Size)
            .Take(query.Size)
            .ToListAsync(cancellationToken);

        List<JobCardDto> jobCardDtos = [];
        
        foreach (var jo in jobObjects)
        {
            var avatar = jo.CompanyAvatars?.GetLatestAvailableAvatar();

            string? avatarLink = null;

            if (avatar is not null)
            {
                avatarLink = await fileStorageService.GetDownloadUrlAsync(FileStorageBucketName.CompanyAvatars, 
                    avatar.GuidIdentifier, avatar.Extension, cancellationToken);
            }
            
            var jobCardDto = new JobCardDto(jo.JobId, jo.CompanyId, avatarLink, jo.CompanyName,
                jo.Locations.Select(l => l.ToLocationDto()).ToList(),
                jo.Title, jo.DateTimePublishedUtc, jo.DateTimeExpiringUtc, jo.SalaryInfo?.ToJobSalaryInfoDto(),
                jo.EmploymentOptionIds.ToList(), jo.ContractTypeIds.ToList(), true);
            
            jobCardDtos.Add(jobCardDto);
        }

        var paginationResponse = new PaginationResponse(query.Page,
            query.Size, count);

        var response = new GetBookmarkedJobsResult(jobCardDtos, paginationResponse);

        return response;
    }
}
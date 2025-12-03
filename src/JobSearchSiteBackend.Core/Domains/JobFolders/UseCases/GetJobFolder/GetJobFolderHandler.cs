using Ardalis.Result;
using AutoMapper;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Companies.Persistence;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims;
using JobSearchSiteBackend.Core.Domains.JobFolders.Dtos;
using JobSearchSiteBackend.Core.Domains.JobFolders.Persistence;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.Caching;
using JobSearchSiteBackend.Core.Services.FileStorage;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.GetJobFolder;

public class GetJobFolderHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context,
    IFileStorageService fileStorageService,
    ICompanyLastVisitedFoldersCacheRepository cacheRepo) : IRequestHandler<GetJobFolderQuery, Result<GetJobFolderResult>>
{
    public async Task<Result<GetJobFolderResult>> Handle(GetJobFolderQuery query,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var dbQuery = context.JobFolders
            .Where(jf => jf.Id == query.Id)
            .Select(jf => new
            {
                Id = jf.Id,
                Name = jf.Name,
                Description = jf.Description,
                ParentId = jf.RelationsWhereThisIsDescendant!
                    .Where(jfr => jfr.Depth == 1)
                    .Select(x => x.AncestorId)
                    .SingleOrDefault(),
                CompanyId = jf.CompanyId,
                CompanyName = jf.Company!.Name,
                CompanyAvatars = jf.Company!.CompanyAvatars,
                ChildrenWithClaimIds = jf.RelationsWhereThisIsAncestor!
                    .Where(jfr => jfr.Depth == 1)
                    .Select(jfr => new
                    {
                        ChildJobFolder = jfr.Descendant!,
                        ClaimIds = jfr.Descendant!.UserJobFolderClaims!
                            .Where(ujfc => ujfc.UserId == currentUserId)
                            .Select(ujfc => ujfc.ClaimId)
                    })
                
            });

        var jobFolderObject = await dbQuery.SingleOrDefaultAsync(cancellationToken);
        
        if (jobFolderObject is null)
            return Result<GetJobFolderResult>.NotFound();

        var claimIdList = await context.JobFolderRelations
            .GetClaimIdsForThisAndAncestors(query.Id, currentUserId)
            .ToListAsync(cancellationToken);

        if (!claimIdList.Contains(JobFolderClaim.CanReadJobs.Id))
            return Result<GetJobFolderResult>.Forbidden();
        
        await cacheRepo.AddLastVisitedAsync(currentUserId.ToString(),
            jobFolderObject.CompanyId.ToString(), jobFolderObject.Id.ToString());

        var lastAvatar = jobFolderObject.CompanyAvatars!.GetLatestAvailableAvatar();
        string? avatarLink = null;
        
        if (lastAvatar is not null)
        {
            avatarLink = await fileStorageService.GetDownloadUrlAsync(FileStorageBucketName.CompanyAvatars,
                lastAvatar.GuidIdentifier, lastAvatar.Extension, cancellationToken);
        }
        
        var jobFolderDetailedDto = new JobFolderDetailedDto(
            jobFolderObject.Id,
            jobFolderObject.Name,
            jobFolderObject.Description,
            null, //todo
            jobFolderObject.ParentId,
            jobFolderObject.CompanyId,
            jobFolderObject.CompanyName,
            avatarLink,
            claimIdList,
            jobFolderObject.ChildrenWithClaimIds
                .Select(c => new JobFolderMinimalDto(c.ChildJobFolder.Id,
                    c.ChildJobFolder.Name, c.ClaimIds.ToList()))
                .ToList()
            );

        var response = new GetJobFolderResult(jobFolderDetailedDto);

        return Result.Success(response);
    }
}
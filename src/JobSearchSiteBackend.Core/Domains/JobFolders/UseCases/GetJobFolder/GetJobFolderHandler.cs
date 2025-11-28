using Ardalis.Result;
using AutoMapper;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
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
    ICompanyLastVisitedFoldersCacheRepository cacheRepo,
    IMapper mapper) : IRequestHandler<GetJobFolderQuery, Result<GetJobFolderResult>>
{
    public async Task<Result<GetJobFolderResult>> Handle(GetJobFolderQuery query,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var jobFolder = await context.JobFolders
            .AsNoTracking()
            .Include(jf => jf.Company)
            .ThenInclude(c => c!.CompanyAvatars!
                .Where(a => !a.IsDeleted && a.IsUploadedSuccessfully)
                .OrderBy(a => a.DateTimeUpdatedUtc))
            .Include(jf => jf.RelationsWhereThisIsDescendant!.Where(x => x.Depth == 1))
            .Include(jf => jf.RelationsWhereThisIsAncestor!.Where(x => x.Depth == 1))
            .ThenInclude(rel => rel.Descendant)
            .Where(jf => jf.Id == query.Id)
            .SingleOrDefaultAsync(cancellationToken);
        
        if (jobFolder is null)
            return Result<GetJobFolderResult>.NotFound();

        var claimIdList = await context.JobFolderRelations
            .GetClaimIdsForThisAndAncestors(query.Id, currentUserId)
            .ToListAsync(cancellationToken);

        if (!claimIdList.Contains(JobFolderClaim.CanReadJobs.Id))
            return Result<GetJobFolderResult>.Forbidden();
        
        await cacheRepo.AddLastVisitedAsync(currentUserId.ToString(),
            jobFolder.CompanyId.ToString(), jobFolder.Id.ToString());

        var lastAvatar = jobFolder.Company!.CompanyAvatars!.LastOrDefault();
        string? companyLogoLink = null;
        
        if (lastAvatar is not null)
        {
            companyLogoLink = await fileStorageService.GetDownloadUrlAsync(FileStorageBucketName.CompanyAvatars,
                lastAvatar.GuidIdentifier, lastAvatar.Extension, cancellationToken);
        }
        
        var jobFolderDetailedDto = new JobFolderDetailedDto(
            jobFolder.Id,
            jobFolder.Name,
            jobFolder.Description,
            null, //todo
            jobFolder.RelationsWhereThisIsDescendant!.Select(x => x.AncestorId).FirstOrDefault(),
            jobFolder.CompanyId,
            jobFolder.Company!.Name,
            companyLogoLink,
            claimIdList,
            mapper.Map<List<JobFolderMinimalDto>>(jobFolder.RelationsWhereThisIsAncestor!.Select(rel => rel.Descendant))
            );

        var response = new GetJobFolderResult(jobFolderDetailedDto);

        return Result.Success(response);
    }
}
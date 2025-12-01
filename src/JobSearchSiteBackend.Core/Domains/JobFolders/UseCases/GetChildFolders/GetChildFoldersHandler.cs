using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims;
using JobSearchSiteBackend.Core.Domains.JobFolders.Dtos;
using JobSearchSiteBackend.Core.Domains.Jobs.Dtos;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using JobSearchSiteBackend.Core.Domains.JobFolders.Persistence;
using JobSearchSiteBackend.Core.Persistence;

namespace JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.GetChildFolders;

public class GetChildFoldersHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<GetChildFoldersQuery, Result<GetChildFoldersResult>>
{
    public async Task<Result<GetChildFoldersResult>> Handle(GetChildFoldersQuery query,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var jobFolder = await context.JobFolders.FindAsync([query.Id], cancellationToken);
        if (jobFolder is null)
            return Result<GetChildFoldersResult>.NotFound();

        var claimIdListForThisAndAncestors = await context.JobFolderRelations
            .GetClaimIdsForThisAndAncestors(query.Id, currentUserId)
            .ToListAsync(cancellationToken);

        if (!claimIdListForThisAndAncestors.Contains(JobFolderClaim.CanReadJobs.Id))
            return Result<GetChildFoldersResult>.Forbidden();

        var queriedChildJobFolders = await context.JobFolderRelations
            .Where(jfc => jfc.AncestorId == query.Id)
            .Where(jfc => jfc.Depth == 1)
            .Select(jfr => new
            {
                JobFolderId = jfr.DescendantId,
                JobFolderName = jfr.Descendant!.Name,
                ClaimIds = context.JobFolderRelations.GetClaimIdsForThisAndAncestors(jfr.DescendantId, currentUserId)
            })
            .ToListAsync(cancellationToken);
        
        var jobFolderMinimalDtos = queriedChildJobFolders
            .Select(anonType => new JobFolderMinimalDto(anonType.JobFolderId,
                anonType.JobFolderName, anonType.ClaimIds.ToList()))
            .ToList();

        var result = new GetChildFoldersResult(jobFolderMinimalDtos);

        return Result.Success(result);
    }
}
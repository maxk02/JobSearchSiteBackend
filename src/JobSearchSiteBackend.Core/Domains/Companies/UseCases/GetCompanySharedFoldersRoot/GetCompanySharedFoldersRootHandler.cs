using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims;
using JobSearchSiteBackend.Core.Domains.JobFolders.Dtos;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanySharedFoldersRoot;

public class GetCompanySharedFoldersRootHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<GetCompanySharedFoldersRootQuery, Result<GetCompanySharedFoldersRootResult>>
{
    public async Task<Result<GetCompanySharedFoldersRootResult>> Handle(GetCompanySharedFoldersRootQuery query,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var jobFolderIdsWherePermissionPresent = context.JobFolders
            .Where(jf  => jf.CompanyId == query.CompanyId)
            .Where(jf =>
                jf.UserJobFolderClaims!.Any(jfc =>
                    jfc.ClaimId == JobFolderClaim.CanReadJobs.Id && jfc.UserId == currentUserId))
            .Select(jf => jf.Id);

        var jobFolderObjectsWithoutDescendants = await context.JobFolders
            .Where(jf => jobFolderIdsWherePermissionPresent.Contains(jf.Id))
            .Where(jf =>
                !jf.RelationsWhereThisIsDescendant!.Any(r =>
                    jobFolderIdsWherePermissionPresent.Contains(r.AncestorId)))
            .GroupBy(jf => new { jf.Id, jf.Name })
            .Select(g => new
            {
                Id = g.Key.Id,
                Name = g.Key.Name,
                ClaimIds = g
                    .SelectMany(jf => jf.RelationsWhereThisIsDescendant!
                        .SelectMany(rel => rel.Descendant!.UserJobFolderClaims!
                            .Where(ujfc => ujfc.UserId == currentUserId)))
                    .Select(ujfc => ujfc.ClaimId)
                    .Distinct()
            })
            .ToListAsync(cancellationToken); // cutting folders whose ancestor is already present in output
        
        var jobFolderMinimalDtos = jobFolderObjectsWithoutDescendants
            .Select(o => new JobFolderMinimalDto(o.Id, o.Name, o.ClaimIds.ToList()))
            .ToList();
        
        var result = new GetCompanySharedFoldersRootResult(jobFolderMinimalDtos);
        
        return Result.Success(result);
    }
}
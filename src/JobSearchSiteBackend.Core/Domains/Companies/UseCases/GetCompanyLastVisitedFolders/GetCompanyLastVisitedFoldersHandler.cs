using Ardalis.Result;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Companies.Dtos;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using JobSearchSiteBackend.Core.Services.Caching;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyLastVisitedFolders;

public class GetCompanyLastVisitedFoldersHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context,
    ICompanyLastVisitedFoldersCacheRepository cacheRepo
    ) : IRequestHandler<GetCompanyLastVisitedFoldersQuery,
    Result<GetCompanyLastVisitedFoldersResult>>
{
    public async Task<Result<GetCompanyLastVisitedFoldersResult>> Handle(
        GetCompanyLastVisitedFoldersQuery query,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var idListFromCache = await cacheRepo
            .GetLastVisitedAsync(currentUserId.ToString(), query.CompanyId.ToString());

        if (idListFromCache.Count == 0)
        {
            var emptyResult = new GetCompanyLastVisitedFoldersResult([]);
            return Result.Success(emptyResult);
        }
        
        var jobFolderObjects = await context.JobFolders
            .Where(jf => jf.CompanyId == query.CompanyId)
            .Where(jf =>
                jf.RelationsWhereThisIsDescendant!
                    .Any(rel => rel.Ancestor!.UserJobFolderClaims!.Any(ujfc =>
                        ujfc.ClaimId == JobFolderClaim.CanReadJobs.Id && ujfc.UserId == currentUserId))
                )
            .Where(jf => idListFromCache.Contains(jf.Id))
            .GroupBy(jf => new { jf.Id, jf.Name })
            .Select(g => new
            {
                Id = g.Key.Id,
                Name = g.Key.Name,
                ClaimIds = g
                    .SelectMany(jf => jf.RelationsWhereThisIsDescendant!
                        .SelectMany(rel => rel.Ancestor!.UserJobFolderClaims!
                            .Where(ujfc => ujfc.UserId == currentUserId)))
                    .Select(ujfc => ujfc.ClaimId)
                    .Distinct()
            })
            .ToListAsync(cancellationToken);

        var jobFolderListItemDtos = jobFolderObjects
            .Select(o => new CompanyJobFolderListItemDto(o.Id, o.Name, o.ClaimIds.ToList()))
            .ToList();
        
        var result = new GetCompanyLastVisitedFoldersResult(jobFolderListItemDtos);

        return Result.Success(result);
    }
}
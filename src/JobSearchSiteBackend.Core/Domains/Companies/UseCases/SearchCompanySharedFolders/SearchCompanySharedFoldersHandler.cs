using Ardalis.Result;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Companies.Dtos;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.SearchCompanySharedFolders;

public class SearchCompanySharedFoldersHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context,
    IMapper mapper) : IRequestHandler<SearchCompanySharedFoldersQuery, Result<SearchCompanySharedFoldersResult>>
{
    public async Task<Result<SearchCompanySharedFoldersResult>> Handle(SearchCompanySharedFoldersQuery query,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var jobFolderListItemDtos = await context.JobFolders
            .Where(jf  => jf.CompanyId == query.CompanyId)
            .Where(jf =>
                jf.UserJobFolderClaims!.Any(jfc =>
                    jfc.ClaimId == JobFolderClaim.CanReadJobs.Id && jfc.UserId == currentUserId))
            .Where(jf => jf.Name!.Contains(query.Query) || jf.Description!.Contains(query.Query))
            .ProjectTo<CompanyJobFolderListItemDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        
        var response = new SearchCompanySharedFoldersResult(jobFolderListItemDtos);
        
        return Result.Success(response);
    }
}
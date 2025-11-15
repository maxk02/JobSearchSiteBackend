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
    IMapper mapper,
    ICompanyLastVisitedFoldersCacheRepository cacheRepo
    ) : IRequestHandler<GetCompanyLastVisitedFoldersRequest,
    Result<GetCompanyLastVisitedFoldersResponse>>
{
    public async Task<Result<GetCompanyLastVisitedFoldersResponse>> Handle(GetCompanyLastVisitedFoldersRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var idListFromCache = await cacheRepo
            .GetLastVisitedAsync(currentUserId.ToString(), request.CompanyId.ToString());
        
        var jobFolderListItemDtos = await context.JobFolders
            .Where(jf => jf.CompanyId == request.CompanyId)
            .Where(jf =>
                jf.UserJobFolderClaims!.Any(jfc =>
                    jfc.ClaimId == JobFolderClaim.CanReadJobs.Id && jfc.UserId == currentUserId))
            .Where(jf => idListFromCache.Contains(jf.Id))
            .ProjectTo<CompanyJobFolderListItemDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        var response = new GetCompanyLastVisitedFoldersResponse(jobFolderListItemDtos);

        return Result.Success(response);
    }
}
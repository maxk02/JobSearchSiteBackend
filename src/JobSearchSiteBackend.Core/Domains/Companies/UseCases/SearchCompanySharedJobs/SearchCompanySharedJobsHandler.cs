using Ardalis.Result;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Companies.Dtos;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.SearchCompanySharedJobs;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.SearchCompanySharedJobs;

public class SearchCompanySharedJobsHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context,
    IMapper mapper) : IRequestHandler<SearchCompanySharedJobsRequest, Result<SearchCompanySharedJobsResponse>>
{
    public async Task<Result<SearchCompanySharedJobsResponse>> Handle(SearchCompanySharedJobsRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var jobListItemDtos = await context.JobFolders
            .Where(jf => jf.CompanyId == request.CompanyId)
            .Where(jf =>
                jf.UserJobFolderClaims!.Any(jfc =>
                    jfc.ClaimId == JobFolderClaim.CanReadJobs.Id && jfc.UserId == currentUserId))
            .SelectMany(jf => jf.Jobs!)
            .Where(j => j.Title.Contains(request.Query) || j.Description!.Contains(request.Query))
            .ProjectTo<CompanyJobListItemDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        
        var response = new SearchCompanySharedJobsResponse(jobListItemDtos);
        
        return Result.Success(response);
    }
}
using Ardalis.Result;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims;
using JobSearchSiteBackend.Core.Domains.JobFolders;
using JobSearchSiteBackend.Core.Domains.JobFolders.Dtos;
using JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.GetChildFolders;
using JobSearchSiteBackend.Core.Persistence;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanySharedFoldersRoot;

public class GetCompanySharedFoldersRootHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context,
    IMapper mapper) : IRequestHandler<GetCompanySharedFoldersRootRequest, Result<GetCompanySharedFoldersRootResponse>>
{
    public async Task<Result<GetCompanySharedFoldersRootResponse>> Handle(GetCompanySharedFoldersRootRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var jobFolderIdsWherePermissionPresent = context.JobFolders
            .Where(jf  => jf.CompanyId == request.CompanyId)
            .Where(jf =>
                jf.UserJobFolderClaims!.Any(jfc =>
                    jfc.ClaimId == JobFolderClaim.CanReadJobs.Id && jfc.UserId == currentUserId))
            .Select(jf => jf.Id);

        var jobFolderDtosWithoutDescendants = await context.JobFolders
            .Where(jf => jobFolderIdsWherePermissionPresent.Contains(jf.Id))
            .Where(jf =>
                !jf.RelationsWhereThisIsDescendant!.Any(r =>
                    jobFolderIdsWherePermissionPresent.Contains(r.AncestorId)))
            .ProjectTo<JobFolderMinimalDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken); // cutting folders whose ancestor is already present in output
        
        var response = new GetCompanySharedFoldersRootResponse(jobFolderDtosWithoutDescendants);
        
        return Result.Success(response);
    }
}
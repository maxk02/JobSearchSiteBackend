using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims;
using JobSearchSiteBackend.Core.Domains.JobFolders.Dtos;
using JobSearchSiteBackend.Core.Domains.Jobs.Dtos;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using JobSearchSiteBackend.Core.Persistence;

namespace JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.GetChildFolders;

public class GetChildFoldersHandler(
    ICurrentAccountService currentAccountService,
    MainDataContext context,
    IMapper mapper) : IRequestHandler<GetChildFoldersQuery, Result<GetChildFoldersResult>>
{
    public async Task<Result<GetChildFoldersResult>> Handle(GetChildFoldersQuery query,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var jobFolder = await context.JobFolders.FindAsync([query.Id], cancellationToken);
        if (jobFolder is null)
            return Result<GetChildFoldersResult>.NotFound();

        var hasReadClaim = await context.JobFolderRelations
            .GetThisOrAncestorWhereUserHasClaim(query.Id, currentUserId,
                JobFolderClaim.CanReadJobs.Id)
            .AnyAsync(cancellationToken);

        if (!hasReadClaim)
            return Result<GetChildFoldersResult>.Forbidden();

        var childJobFolderDtos = await context.JobFolderRelations
            .Where(jfc => jfc.AncestorId == query.Id)
            .Where(jfc => jfc.Depth == 1)
            .ProjectTo<JobFolderMinimalDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new GetChildFoldersResult(childJobFolderDtos);
    }
}
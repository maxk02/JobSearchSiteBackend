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
    IMapper mapper) : IRequestHandler<GetChildFoldersRequest, Result<GetChildFoldersResponse>>
{
    public async Task<Result<GetChildFoldersResponse>> Handle(GetChildFoldersRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();

        var jobFolder = await context.JobFolders.FindAsync([request.Id], cancellationToken);
        if (jobFolder is null)
            return Result<GetChildFoldersResponse>.NotFound();

        var hasReadClaim = await context.JobFolderRelations
            .GetThisOrAncestorWhereUserHasClaim(request.Id, currentUserId,
                JobFolderClaim.CanReadJobs.Id)
            .AnyAsync(cancellationToken);

        if (!hasReadClaim)
            return Result<GetChildFoldersResponse>.Forbidden();

        var childJobFolderDtos = await context.JobFolderRelations
            .Where(jfc => jfc.AncestorId == request.Id)
            .Where(jfc => jfc.Depth == 1)
            .ProjectTo<JobFolderDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new GetChildFoldersResponse(childJobFolderDtos);
    }
}
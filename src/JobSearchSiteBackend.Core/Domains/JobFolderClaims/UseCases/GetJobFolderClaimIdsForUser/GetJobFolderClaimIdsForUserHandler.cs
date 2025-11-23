using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.CompanyClaims;
using JobSearchSiteBackend.Core.Domains.JobFolders;
using JobSearchSiteBackend.Core.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result;
using JobSearchSiteBackend.Core.Persistence;

namespace JobSearchSiteBackend.Core.Domains.JobFolderClaims.UseCases.GetJobFolderClaimIdsForUser;

public class GetJobFolderClaimIdsForUserHandler(ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<GetJobFolderClaimIdsForUserQuery, Result<ICollection<long>>>
{
    public async Task<Result<ICollection<long>>> Handle(GetJobFolderClaimIdsForUserQuery query,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = currentAccountService.GetIdOrThrow();
        
        var currentUserClaimIdsOnThisAndAncestors = await context.JobFolderRelations
            .GetClaimIdsForThisAndAncestors(query.FolderId, currentUserId)
            .ToListAsync(cancellationToken);
        
        if (query.UserId == currentUserId)
            return currentUserClaimIdsOnThisAndAncestors;
        
        if (!currentUserClaimIdsOnThisAndAncestors.Contains(JobFolderClaim.IsAdmin.Id))
            return Result<ICollection<long>>.Forbidden("Current user is not a folder admin.");
        
        var targetUserClaimIdsOnThisAndAncestors = await context.JobFolderRelations
            .GetClaimIdsForThisAndAncestors(query.FolderId, query.UserId)
            .ToListAsync(cancellationToken);

        var visibleClaimIds =
            currentUserClaimIdsOnThisAndAncestors
                .Intersect(targetUserClaimIdsOnThisAndAncestors)
                .ToList();
        
        return visibleClaimIds;
    }
}
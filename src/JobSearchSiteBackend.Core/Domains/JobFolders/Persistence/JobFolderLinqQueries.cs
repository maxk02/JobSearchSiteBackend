using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.JobFolders.Persistence;

public static class JobFolderLinqQueries
{
    public static IQueryable<JobFolderRelation> GetThisOrAncestorsWhereUserHasClaims(
        this DbSet<JobFolderRelation> dbSet,
        long jobFolderId, long userProfileId, ICollection<long> claimIds)
    {
        return dbSet
            .Where(jobFolderClosure => jobFolderClosure.DescendantId == jobFolderId)
            .Where(jobFolderClosure => jobFolderClosure.Ancestor!.UserJobFolderClaims!
                .Any(userJobFolderClaim =>
                    userJobFolderClaim.UserId == userProfileId &&
                    claimIds.Contains(userJobFolderClaim.ClaimId)));
    }

    public static IQueryable<JobFolderRelation> GetThisOrAncestorWhereUserHasClaim(
        this DbSet<JobFolderRelation> dbSet,
        long jobFolderId, long userProfileId, long claimId)
    {
        return dbSet
            .Where(jobFolderClosure => jobFolderClosure.DescendantId == jobFolderId)
            .Where(jobFolderClosure => jobFolderClosure.Ancestor!.UserJobFolderClaims!
                .Any(userJobFolderClaim =>
                    userJobFolderClaim.UserId == userProfileId &&
                    userJobFolderClaim.ClaimId == claimId));
    }
    
    public static IQueryable<long> GetClaimIdsForThisAndAncestors(
        this DbSet<JobFolderRelation> dbSet,
        long jobFolderId, long userProfileId)
    {
        return dbSet
            .AsNoTracking()
            .Where(jfc => jfc.DescendantId == jobFolderId)
            .SelectMany(jfc => jfc.Ancestor!.UserJobFolderClaims!)
            .Where(ujfc => ujfc.UserId == userProfileId)
            .Select(ujfc => ujfc.ClaimId);
    }
}
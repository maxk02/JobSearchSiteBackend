using Microsoft.EntityFrameworkCore;

namespace Core.Domains.JobFolders;

public static class JobFolderQueries
{
    public static IQueryable<JobFolderClosure> GetThisOrAncestorsWhereUserHasClaims(
        this DbSet<JobFolderClosure> dbSet,
        long jobFolderId, long userProfileId, ICollection<long> claimIds)
    {
        return dbSet
            .Where(jobFolderClosure => jobFolderClosure.DescendantId == jobFolderId)
            .Where(jobFolderClosure => jobFolderClosure.Ancestor!.UserJobFolderClaims!
                .Any(userJobFolderClaim =>
                    userJobFolderClaim.UserId == userProfileId &&
                    claimIds.Contains(userJobFolderClaim.ClaimId)));
    }

    public static IQueryable<JobFolderClosure> GetThisOrAncestorWhereUserHasClaim(
        this DbSet<JobFolderClosure> dbSet,
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
        this DbSet<JobFolderClosure> dbSet,
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
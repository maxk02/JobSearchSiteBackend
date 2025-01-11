using Core.Domains.JobFolderClaims;
using Microsoft.EntityFrameworkCore;

namespace Core.Domains.JobFolders;

public static class JobFolderQueries
{
    public static IQueryable<JobFolderClosure> GetThisOrAncestorsWhereUserHasClaims(
        this DbSet<JobFolderClosure> dbSet,
        long jobFolderId, long userProfileId, ICollection<long> claimIds)
    {
        return dbSet
            .Include(jobFolderClosure => jobFolderClosure.Ancestor)
            .ThenInclude(ancestralJobFolder => ancestralJobFolder!.UserJobFolderClaims)
            .Where(jobFolderClosure => jobFolderClosure.DescendantId == jobFolderId)
            .Where(jobFolderClosure => jobFolderClosure.Ancestor!.UserJobFolderClaims!
                .Any(userJobFolderClaim =>
                    userJobFolderClaim.UserId == userProfileId &&
                    claimIds.Contains(userJobFolderClaim.ClaimId)));
    }

    public static IQueryable<JobFolderClosure> GetThisOrAncestorsWhereUserHasClaim(
        this DbSet<JobFolderClosure> dbSet,
        long jobFolderId, long userProfileId, long claimId)
    {
        return dbSet
            .Include(jobFolderClosure => jobFolderClosure.Ancestor)
            .ThenInclude(ancestralJobFolder => ancestralJobFolder!.UserJobFolderClaims)
            .Where(jobFolderClosure => jobFolderClosure.DescendantId == jobFolderId)
            .Where(jobFolderClosure => jobFolderClosure.Ancestor!.UserJobFolderClaims!
                .Any(userJobFolderClaim =>
                    userJobFolderClaim.UserId == userProfileId &&
                    userJobFolderClaim.ClaimId == claimId));
    }
    
    public static IQueryable<long> GetDistinctClaimIdsForThisAndAncestors(
        this DbSet<JobFolderClosure> dbSet,
        long jobFolderId, long userProfileId)
    {
        return dbSet
            .Include(jfc => jfc.Ancestor)
            .ThenInclude(jobFolder => jobFolder!.UserJobFolderClaims)
            .Where(jfc => jfc.DescendantId == jobFolderId)
            .SelectMany(jfc => jfc.Ancestor!.UserJobFolderClaims!)
            .Where(ujfc => ujfc.UserId == userProfileId)
            .Select(ujfc => ujfc.ClaimId)
            .Distinct();
    }
}
using Microsoft.EntityFrameworkCore;

namespace JobSearchSiteBackend.Core.Domains.UserProfiles.Persistence;

public static class UserAvatarLinqQueries
{
    public static IQueryable<UserAvatar> FilterLatestAvailableAvatar(
        this IQueryable<UserAvatar> query, long userProfileId)
    {
        return query
            .AsNoTracking()
            .Where(av => av.UserId == userProfileId)
            .Where(av => !av.IsDeleted)
            .Where(av => av.IsUploadedSuccessfully)
            .OrderByDescending(av => av.DateTimeUpdatedUtc)
            .Take(1);
    }
    
    public static IEnumerable<UserAvatar> FilterLatestAvailableAvatar(
        this IEnumerable<UserAvatar> query, long userProfileId)
    {
        return query
            .Where(av => av.UserId == userProfileId)
            .Where(av => !av.IsDeleted)
            .Where(av => av.IsUploadedSuccessfully)
            .OrderByDescending(av => av.DateTimeUpdatedUtc)
            .Take(1);
    }
}
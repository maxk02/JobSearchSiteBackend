namespace JobSearchSiteBackend.Core.Domains.UserProfiles.Persistence;

public static class UserAvatarLinqQueries
{
    public static UserAvatar? GetLatestAvailableAvatar(
        this ICollection<UserAvatar> avatarCollection)
    {
        if (avatarCollection.Count == 0)
            return null;

        if (avatarCollection.Any(av => av.UserId != avatarCollection.First().UserId))
        {
            throw new InvalidOperationException("Avatars must be already filtered by user.");
        }
        
        return avatarCollection
            .Where(av => !av.IsDeleted)
            .Where(av => av.IsUploadedSuccessfully)
            .OrderByDescending(av => av.DateTimeUpdatedUtc)
            .FirstOrDefault();
    }
}
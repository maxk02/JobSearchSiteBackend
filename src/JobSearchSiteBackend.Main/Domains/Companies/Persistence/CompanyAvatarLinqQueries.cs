namespace JobSearchSiteBackend.Core.Domains.Companies.Persistence;

public static class CompanyAvatarLinqQueries
{
    public static CompanyAvatar? GetLatestAvailableAvatar(
        this ICollection<CompanyAvatar> avatarCollection)
    {
        if (avatarCollection.Count == 0)
            return null;

        if (avatarCollection.Any(av => av.CompanyId != avatarCollection.First().CompanyId))
        {
            throw new InvalidOperationException("Avatars must be already filtered by company.");
        }
        
        return avatarCollection
            .Where(av => !av.IsDeleted)
            .Where(av => av.IsUploadedSuccessfully)
            .OrderByDescending(av => av.DateTimeUpdatedUtc)
            .FirstOrDefault();
    }
    
    
}
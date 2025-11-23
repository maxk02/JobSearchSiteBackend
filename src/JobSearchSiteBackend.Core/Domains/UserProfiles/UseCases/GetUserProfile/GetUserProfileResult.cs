namespace JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.GetUserProfile;

public record GetUserProfileResult(string FirstName, string LastName, string Email, string? Phone, string? AvatarLink);
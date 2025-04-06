namespace Core.Domains.UserProfiles.UseCases.GetUserProfile;

public record GetUserProfileResponse(string FirstName, string LastName, string Email, string? Phone);
namespace JobSearchSiteBackend.API.Controllers.UserProfiles.Dtos;

public record GetUserProfileResponse(string FirstName, string LastName, string Email,
    string? Phone, string? AvatarLink, bool IsReceivingApplicationStatusUpdates);
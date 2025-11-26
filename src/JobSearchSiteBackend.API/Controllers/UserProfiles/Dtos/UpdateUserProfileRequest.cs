namespace JobSearchSiteBackend.API.Controllers.UserProfiles.Dtos;

public record UpdateUserProfileRequest(string? FirstName, string? LastName,
    string? Phone, bool? IsReceivingApplicationStatusUpdates);
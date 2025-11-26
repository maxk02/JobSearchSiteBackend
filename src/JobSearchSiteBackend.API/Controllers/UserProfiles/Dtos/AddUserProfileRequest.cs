namespace JobSearchSiteBackend.API.Controllers.UserProfiles.Dtos;

public sealed record AddUserProfileRequest(string FirstName, string LastName, string? Phone);
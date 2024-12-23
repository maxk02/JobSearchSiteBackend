using Core.Domains._Shared.ValueEntities;

namespace Core.Domains.UserProfiles.UseCases.UpdateUserProfile;

public record UpdateUserProfileRequest(long Id, string? FirstName, string? MiddleName, string? LastName,
    DateOnly? DateOfBirth, string? Email, Phone? Phone);
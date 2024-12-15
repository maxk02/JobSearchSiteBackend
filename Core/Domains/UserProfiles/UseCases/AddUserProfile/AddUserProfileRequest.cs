using Core.Domains._Shared.ValueEntities;

namespace Core.Domains.UserProfiles.UseCases.AddUserProfile;

public sealed record AddUserProfileRequest(long AccountId, string FirstName, string? MiddleName, string LastName,
    DateOnly? DateOfBirth, string Email, Phone? Phone, string? Bio);
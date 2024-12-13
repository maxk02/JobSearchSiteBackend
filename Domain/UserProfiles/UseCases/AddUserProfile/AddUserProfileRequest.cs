using Domain._Shared.ValueEntities;

namespace Domain.UserProfiles.UseCases.AddUserProfile;

public sealed record AddUserProfileRequest(long AccountId, string FirstName, string? MiddleName, string LastName,
    DateOnly? DateOfBirth, string Email, Phone? Phone, string? Bio);
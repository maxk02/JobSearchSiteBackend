using Domain._Shared.ValueEntities;

namespace Domain.UserProfiles.UseCases.UpdateUserProfile;

public record UpdateUserProfileRequest(long Id, string? FirstName, string? MiddleName, string? LastName,
    DateOnly? DateOfBirth, string? Email, Phone? Phone, string? Bio);
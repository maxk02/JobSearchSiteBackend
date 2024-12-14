using Core._Shared.ValueEntities;

namespace Core.UserProfiles.UseCases.GetUserProfileById;

public record GetUserProfileByIdResponse(string FirstName, string? MiddleName, string LastName,
    DateOnly? DateOfBirth, string Email, Phone? Phone, string? Bio);
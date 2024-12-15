using Core.Domains._Shared.ValueEntities;

namespace Core.Domains.UserProfiles.UseCases.GetUserProfileById;

public record GetUserProfileByIdResponse(string FirstName, string? MiddleName, string LastName,
    DateOnly? DateOfBirth, string Email, Phone? Phone, string? Bio);
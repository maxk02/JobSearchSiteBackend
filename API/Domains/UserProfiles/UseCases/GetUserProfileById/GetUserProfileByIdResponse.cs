using API.Domains._Shared.ValueEntities;

namespace API.Domains.UserProfiles.UseCases.GetUserProfileById;

public record GetUserProfileByIdResponse(string FirstName, string? MiddleName, string LastName,
    DateOnly? DateOfBirth, string Email, Phone? Phone, string? Bio);
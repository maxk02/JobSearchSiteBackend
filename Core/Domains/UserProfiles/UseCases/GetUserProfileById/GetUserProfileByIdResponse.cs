namespace Core.Domains.UserProfiles.UseCases.GetUserProfileById;

public record GetUserProfileByIdResponse(string FirstName, string? MiddleName, string LastName,
    DateOnly? DateOfBirth, string Email, string? Phone);
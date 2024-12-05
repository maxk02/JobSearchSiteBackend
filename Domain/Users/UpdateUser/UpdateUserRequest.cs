using Domain._Shared.ValueEntities;

namespace Domain.Users.UpdateUser;

public record UpdateUserRequest(string AccountId, string? FirstName, string? MiddleName, string? LastName,
    DateOnly? DateOfBirth, string? Email, Phone? Phone, string? Bio);
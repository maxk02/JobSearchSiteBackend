using Domain._Shared.ValueEntities;

namespace Domain.Users.CreateUser;

public sealed record CreateUserRequest(string AccountId, string FirstName, string? MiddleName, string LastName,
    DateOnly? DateOfBirth, string Email, Phone? Phone, string? Bio);
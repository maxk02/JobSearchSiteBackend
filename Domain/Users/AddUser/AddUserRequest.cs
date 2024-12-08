using Domain._Shared.ValueEntities;

namespace Domain.Users.AddUser;

public sealed record AddUserRequest(string AccountId, string FirstName, string? MiddleName, string LastName,
    DateOnly? DateOfBirth, string Email, Phone? Phone, string? Bio);
using Domain._Shared.ValueEntities;

namespace Domain.Users.CreateUser;

public sealed record CreateUserRequest(string Email, string FirstName,
    string MiddleName, string LastName, DateOnly DateOfBirth, Phone Phone, string Bio);
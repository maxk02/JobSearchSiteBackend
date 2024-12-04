using Domain._Shared.ValueEntities;

namespace Domain.Entities.Users.CreateUser;

public sealed record CreateUserRequest(string Email, string Password, string FirstName,
    string MiddleName, string LastName, DateOnly DateOfBirth, Phone Phone, string Bio);
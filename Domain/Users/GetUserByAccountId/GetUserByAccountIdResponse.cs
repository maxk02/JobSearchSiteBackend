using Domain._Shared.ValueEntities;

namespace Domain.Users.GetUserByAccountId;

public record GetUserByAccountIdResponse(string FirstName, string? MiddleName, string LastName,
    DateOnly? DateOfBirth, string Email, Phone? Phone, string? Bio);
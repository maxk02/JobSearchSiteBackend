using Ardalis.Result;
using Core.Domains._Shared.UseCaseStructure;

namespace API.Controllers.UserProfiles;

public record UpdateUserProfileRequestDto(string? FirstName, string? MiddleName, string? LastName,
    DateOnly? DateOfBirth, string? Email, string? Phone) : IRequest<Result>;
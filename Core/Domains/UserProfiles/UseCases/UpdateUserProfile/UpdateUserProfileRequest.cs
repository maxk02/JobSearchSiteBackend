using Ardalis.Result;
using Core.Domains._Shared.UseCaseStructure;

namespace Core.Domains.UserProfiles.UseCases.UpdateUserProfile;

public record UpdateUserProfileRequest(long Id, string? FirstName, string? MiddleName, string? LastName,
    DateOnly? DateOfBirth, string? Email, string? Phone) : IRequest<Result>;
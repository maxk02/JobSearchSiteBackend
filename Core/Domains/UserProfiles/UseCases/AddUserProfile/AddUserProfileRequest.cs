using Ardalis.Result;
using Core.Domains._Shared.UseCaseStructure;

namespace Core.Domains.UserProfiles.UseCases.AddUserProfile;

public sealed record AddUserProfileRequest(long AccountId, string FirstName, string? MiddleName, string LastName,
    DateOnly? DateOfBirth, string Email, string? Phone) : IRequest<Result<AddUserProfileResponse>>;
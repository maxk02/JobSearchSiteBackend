using Core.Domains._Shared.UseCaseStructure;
using Core.Domains._Shared.ValueEntities;
using Shared.Result;

namespace Core.Domains.UserProfiles.UseCases.AddUserProfile;

public sealed record AddUserProfileRequest(long AccountId, string FirstName, string? MiddleName, string LastName,
    DateOnly? DateOfBirth, string Email, Phone? Phone) : IRequest<Result<AddUserProfileResponse>>;
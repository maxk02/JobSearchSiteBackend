using Ardalis.Result;
using Core.Domains._Shared.UseCaseStructure;

namespace Core.Domains.UserProfiles.UseCases.AddUserProfile;

public sealed record AddUserProfileRequest(string FirstName, string LastName, 
    string Email, string? Phone) : IRequest<Result<AddUserProfileResponse>>;
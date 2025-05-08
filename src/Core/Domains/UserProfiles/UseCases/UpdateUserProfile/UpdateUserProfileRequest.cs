using Ardalis.Result;
using Core.Domains._Shared.UseCaseStructure;

namespace Core.Domains.UserProfiles.UseCases.UpdateUserProfile;

public record UpdateUserProfileRequest(string? FirstName, string? LastName, string? Phone) : IRequest<Result>;
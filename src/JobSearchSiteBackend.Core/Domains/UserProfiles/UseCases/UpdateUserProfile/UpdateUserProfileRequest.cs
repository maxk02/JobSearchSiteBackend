using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.UpdateUserProfile;

public record UpdateUserProfileRequest(string? FirstName, string? LastName, string? Phone) : IRequest<Result>;
using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.AddUserProfile;

public sealed record AddUserProfileCommand(string FirstName, string LastName, 
    string Email, string? Phone) : IRequest<Result<AddUserProfileResult>>;
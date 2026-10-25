using Ardalis.Result;
using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Main.Domains.UserProfiles.UseCases.AddUserProfile;

public sealed record AddUserProfileCommand(string FirstName, string LastName, string? Phone) : IRequest<Result<AddUserProfileResult>>;
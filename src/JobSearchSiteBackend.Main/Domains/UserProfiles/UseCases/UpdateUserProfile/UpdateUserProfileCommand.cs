using Ardalis.Result;
using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Main.Domains.UserProfiles.UseCases.UpdateUserProfile;

public record UpdateUserProfileCommand(string? FirstName, string? LastName, string? Phone,
    bool? IsReceivingApplicationStatusUpdates) : IRequest<Result>;
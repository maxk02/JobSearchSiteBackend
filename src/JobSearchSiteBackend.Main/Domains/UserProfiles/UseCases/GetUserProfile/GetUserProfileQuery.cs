using Ardalis.Result;
using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Main.Domains.UserProfiles.UseCases.GetUserProfile;

public record GetUserProfileQuery : IRequest<Result<GetUserProfileResult>>;
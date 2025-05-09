using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.GetUserProfile;

public record GetUserProfileRequest : IRequest<Result<GetUserProfileResponse>>;
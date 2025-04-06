using Ardalis.Result;
using Core.Domains._Shared.UseCaseStructure;

namespace Core.Domains.UserProfiles.UseCases.GetUserProfile;

public record GetUserProfileRequest : IRequest<Result<GetUserProfileResponse>>;
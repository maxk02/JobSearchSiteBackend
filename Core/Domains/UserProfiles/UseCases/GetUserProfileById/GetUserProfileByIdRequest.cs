using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.UserProfiles.UseCases.GetUserProfileById;

public record GetUserProfileByIdRequest(long Id) : IRequest<Result<GetUserProfileByIdResponse>>;
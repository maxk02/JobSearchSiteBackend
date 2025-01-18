using Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace Core.Domains.UserProfiles.UseCases.GetUserProfileById;

public record GetUserProfileByIdRequest(long Id) : IRequest<Result<GetUserProfileByIdResponse>>;
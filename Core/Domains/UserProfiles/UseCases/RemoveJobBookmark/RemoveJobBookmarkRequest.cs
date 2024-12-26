using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.UserProfiles.UseCases.RemoveJobBookmark;

public record RemoveJobBookmarkRequest(long UserId, long JobId) : IRequest<Result>;
using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.UserProfiles.UseCases.AddJobBookmark;

public record AddJobBookmarkRequest(long UserId, long JobId) : IRequest<Result>;
using Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace Core.Domains.UserProfiles.UseCases.DeleteJobBookmark;

public record DeleteJobBookmarkRequest(long UserId, long JobId) : IRequest<Result>;
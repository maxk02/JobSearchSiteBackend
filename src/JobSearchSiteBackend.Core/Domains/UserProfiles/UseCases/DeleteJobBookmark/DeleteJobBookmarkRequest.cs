using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.DeleteJobBookmark;

public record DeleteJobBookmarkRequest(long JobId) : IRequest<Result>;
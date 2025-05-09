using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.AddJobBookmark;

public record AddJobBookmarkRequest(long JobId) : IRequest<Result>;
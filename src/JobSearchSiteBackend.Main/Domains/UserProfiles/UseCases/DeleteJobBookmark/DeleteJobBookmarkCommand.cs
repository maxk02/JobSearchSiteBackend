using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Main.Domains.UserProfiles.UseCases.DeleteJobBookmark;

public record DeleteJobBookmarkCommand(long JobId) : IRequest<Result>;
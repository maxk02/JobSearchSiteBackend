using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Main.Domains.UserProfiles.UseCases.AddJobBookmark;

public record AddJobBookmarkCommand(long JobId) : IRequest<Result>;
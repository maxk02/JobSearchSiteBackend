using Ardalis.Result;
using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Main.Domains.Locations.UseCases.GetLocations;

namespace JobSearchSiteBackend.Main.Domains.PersonalFiles.UseCases.GetDownloadLink;

public record GetDownloadLinkQuery(long FileId) : IRequest<Result<GetDownloadLinkResult>>;
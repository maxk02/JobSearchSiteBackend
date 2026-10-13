using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using JobSearchSiteBackend.Core.Domains.Locations.UseCases.GetLocations;

namespace JobSearchSiteBackend.Core.Domains.PersonalFiles.UseCases.GetDownloadLink;

public record GetDownloadLinkQuery(long FileId) : IRequest<Result<GetDownloadLinkResult>>;
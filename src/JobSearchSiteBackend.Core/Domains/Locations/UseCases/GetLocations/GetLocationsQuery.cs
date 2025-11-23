using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.Locations.UseCases.GetLocations;

public record GetLocationsQuery(long CountryId, string Query) : IRequest<Result<GetLocationsResult>>;
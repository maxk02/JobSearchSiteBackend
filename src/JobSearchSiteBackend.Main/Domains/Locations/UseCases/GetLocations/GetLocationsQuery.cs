using Ardalis.Result;
using JobSearchSiteBackend.Main.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Main.Domains.Locations.UseCases.GetLocations;

public record GetLocationsQuery(long CountryId, string Query, bool? IsConcrete, int Size) : IRequest<Result<GetLocationsResult>>;
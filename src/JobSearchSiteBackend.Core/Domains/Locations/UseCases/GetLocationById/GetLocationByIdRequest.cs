using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.Locations.UseCases.GetLocationById;

public record GetLocationByIdRequest(long Id) : IRequest<Result<GetLocationByIdResponse>>;
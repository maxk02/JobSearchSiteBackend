using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using AutoMapper;
using JobSearchSiteBackend.API.Controllers.Locations.Dtos;
using JobSearchSiteBackend.Core.Domains.Locations.UseCases.GetLocations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobSearchSiteBackend.API.Controllers.Locations;

[ApiController]
[Route("api/locations")]
[Authorize]
public class LocationsController(IMapper mapper) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<GetLocationsResponse>> GetLocations(
        [FromQuery] GetLocationsRequest request,
        [FromServices] GetLocationsHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetLocationsQuery(request.CountryId, request.Query, request.Size);
        
        var result = await handler.Handle(query, cancellationToken);
        
        return this.ToActionResult(result.Map(x => mapper.Map<GetLocationsResponse>(x)));
    }
}
using Ardalis.Result.AspNetCore;
using Core.Domains.Locations.UseCases.GetLocations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobSearchSiteBackend.API.Controllers.Locations;

[ApiController]
[Route("api/locations")]
[Authorize]
public class LocationsController : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<GetLocationsResponse>> GetLocations(
        [FromQuery] GetLocationsRequest request,
        [FromServices] GetLocationsHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
}
using Ardalis.Result.AspNetCore;
using Core.Domains.CompanyClaims.UseCases.GetCompanyClaimIdsForUser;
using Core.Domains.CompanyClaims.UseCases.UpdateCompanyClaimIdsForUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CompanyClaimsController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ICollection<long>>> GetCompanyClaimIdsForUser(
        [FromQuery] GetCompanyClaimIdsForUserRequest request,
        [FromServices] GetCompanyClaimIdsForUserHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpPatch]
    public async Task<ActionResult> UpdateCompanyClaimIdsForUser(
        [FromBody] UpdateCompanyClaimIdsForUserRequest request,
        [FromServices] UpdateCompanyClaimIdsForUserHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
}
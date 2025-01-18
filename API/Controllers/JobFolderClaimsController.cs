using Ardalis.Result.AspNetCore;
using Core.Domains.CompanyClaims.UseCases.GetCompanyClaimIdsForUser;
using Core.Domains.CompanyClaims.UseCases.UpdateCompanyClaimIdsForUser;
using Core.Domains.JobFolderClaims.UseCases.GetJobFolderClaimIdsForUser;
using Core.Domains.JobFolderClaims.UseCases.UpdateJobFolderClaimIdsForUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class JobFolderClaimsController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ICollection<long>>> GetJobFolderClaimIdsForUser(
        [FromQuery] GetJobFolderClaimIdsForUserRequest request,
        [FromServices] GetJobFolderClaimIdsForUserHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpPatch]
    public async Task<ActionResult> UpdateJobFolderClaimIdsForUser(
        [FromBody] UpdateJobFolderClaimIdsForUserRequest request,
        [FromServices] UpdateJobFolderClaimIdsForUserHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
}
using Ardalis.Result.AspNetCore;
using AutoMapper;
using JobSearchSiteBackend.Core.Domains.CompanyClaims.UseCases.GetCompanyClaimIdsForUser;
using JobSearchSiteBackend.Core.Domains.CompanyClaims.UseCases.UpdateCompanyClaimIdsForUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobSearchSiteBackend.API.Controllers.CompanyClaims;

[ApiController]
[Route("api/company-claims")]
[Authorize]
public class CompanyClaimsController(IMapper mapper) : ControllerBase
{
    [HttpGet]
    [Route("/company/{companyId:long:min(1)}/user/{userId:long:min(1)}")]
    public async Task<ActionResult<ICollection<long>>> GetCompanyClaimIdsForUser(
        [FromRoute] long companyId, [FromRoute] long userId,
        [FromServices] GetCompanyClaimIdsForUserHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new GetCompanyClaimIdsForUserRequest(userId, companyId);
        
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpPatch("/company/{companyId:long:min(1)}/user/{userId:long:min(1)}")]
    public async Task<ActionResult> UpdateCompanyClaimIdsForUser(
        [FromRoute] long companyId, [FromRoute] long userId,
        [FromBody] UpdateCompanyClaimIdsForUserRequestDto requestDto,
        [FromServices] UpdateCompanyClaimIdsForUserHandler handler,
        CancellationToken cancellationToken)
    {
        var request = mapper.Map<UpdateCompanyClaimIdsForUserRequest>(requestDto, opt =>
        {
            opt.Items["CompanyId"] = companyId;
            opt.Items["UserId"] = userId;
        });
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
}
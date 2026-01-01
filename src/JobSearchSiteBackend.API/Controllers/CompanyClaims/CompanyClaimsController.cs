using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using AutoMapper;
using JobSearchSiteBackend.API.Controllers.CompanyClaims.Dtos;
using JobSearchSiteBackend.Core.Domains.CompanyClaims.UseCases.GetCompanyClaimIdsForUser;
using JobSearchSiteBackend.Core.Domains.CompanyClaims.UseCases.GetCompanyClaimsOverview;
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
    [Route("company/{companyId:long:min(1)}/user/{userId:long:min(1)}/claim-ids")]
    public async Task<ActionResult<GetCompanyClaimIdsForUserResponse>> GetCompanyClaimIdsForUser(
        [FromRoute] long companyId,
        [FromRoute] long userId,
        [FromServices] GetCompanyClaimIdsForUserHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetCompanyClaimIdsForUserQuery(userId, companyId);
        
        var result = await handler.Handle(query, cancellationToken);
        
        return this.ToActionResult(result.Map(x => mapper.Map<GetCompanyClaimIdsForUserResponse>(x)));
    }
    
    [HttpGet]
    [Route("company/{companyId:long:min(1)}")]
    public async Task<ActionResult<GetCompanyClaimsOverviewResponse>> GetCompanyClaimsOverview(
        [FromRoute] long companyId,
        [FromQuery] GetCompanyClaimsOverviewRequest request,
        [FromServices] GetCompanyClaimsOverviewHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetCompanyClaimsOverviewQuery(companyId, request.CompanyClaimIds,
            request.UserQuery, request.Page, request.Size);
        
        var result = await handler.Handle(query, cancellationToken);
        
        return this.ToActionResult(result.Map(x => mapper.Map<GetCompanyClaimsOverviewResponse>(x)));
    }
    
    [HttpPut]
    [Route("company/{companyId:long:min(1)}/user/{userId:long:min(1)}/claim-ids")]
    public async Task<ActionResult> UpdateCompanyClaimIdsForUser(
        [FromRoute] long companyId,
        [FromRoute] long userId,
        [FromBody] UpdateCompanyClaimIdsForUserRequest request,
        [FromServices] UpdateCompanyClaimIdsForUserHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdateCompanyClaimIdsForUserCommand(userId, companyId, request.CompanyClaimIds);
        
        var result = await handler.Handle(command, cancellationToken);
        
        return this.ToActionResult(result);
    }
}
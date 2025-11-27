using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using AutoMapper;
using JobSearchSiteBackend.API.Controllers.JobFolderClaims.Dtos;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims.UseCases.GetJobFolderClaimIdsForUser;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims.UseCases.GetJobFolderClaimsOverview;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims.UseCases.UpdateJobFolderClaimIdsForUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobSearchSiteBackend.API.Controllers.JobFolderClaims;

[ApiController]
[Route("api/job-folder-claims")]
[Authorize]
public class JobFolderClaimsController(IMapper mapper) : ControllerBase
{
    [HttpGet]
    [Route("folder/{jobFolderId:long:min(1)}/user/{userId:long:min(1)}")]
    public async Task<ActionResult<GetJobFolderClaimIdsForUserResponse>> GetJobFolderClaimIdsForUser(
        [FromRoute] long jobFolderId,
        [FromRoute] long userId,
        [FromServices] GetJobFolderClaimIdsForUserHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetJobFolderClaimIdsForUserQuery(userId, jobFolderId);
        
        var result = await handler.Handle(query, cancellationToken);
        
        return this.ToActionResult(result.Map(x => mapper.Map<GetJobFolderClaimIdsForUserResponse>(x)));
    }
    
    [HttpGet]
    [Route("folder/{jobFolderId:long:min(1)}")]
    public async Task<ActionResult<GetJobFolderClaimsOverviewResponse>> GetJobFolderClaimsOverview(
        [FromRoute] long jobFolderId,
        [FromQuery] GetJobFolderClaimsOverviewRequest request,
        [FromServices] GetJobFolderClaimsOverviewHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetJobFolderClaimsOverviewQuery(jobFolderId, request.UserQuery,
            request.JobFolderClaimIds, request.Page, request.Size);
        
        var result = await handler.Handle(query, cancellationToken);
        
        return this.ToActionResult(result.Map(x => mapper.Map<GetJobFolderClaimsOverviewResponse>(x)));
    }
    
    [HttpPut]
    [Route("folder/{jobFolderId:long:min(1)}/user/{userId:long:min(1)}")]
    public async Task<ActionResult> UpdateJobFolderClaimIdsForUser(
        [FromRoute] long jobFolderId,
        [FromRoute] long userId,
        [FromBody] UpdateJobFolderClaimIdsForUserRequest request,
        [FromServices] UpdateJobFolderClaimIdsForUserHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdateJobFolderClaimIdsForUserCommand(userId, jobFolderId, request.JobFolderClaimIds);
        
        var result = await handler.Handle(command, cancellationToken);
        
        return this.ToActionResult(result);
    }
}
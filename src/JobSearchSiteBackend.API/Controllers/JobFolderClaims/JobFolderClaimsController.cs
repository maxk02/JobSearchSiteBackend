using Ardalis.Result.AspNetCore;
using AutoMapper;
using JobSearchSiteBackend.API.Controllers.JobFolderClaims.Dtos;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims.UseCases.GetJobFolderClaimIdsForUser;
using JobSearchSiteBackend.Core.Domains.JobFolderClaims.UseCases.UpdateJobFolderClaimIdsForUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobSearchSiteBackend.API.Controllers.JobFolderClaims;

[ApiController]
[Route("api/job-folder-claims")]
[Authorize]
public class JobFolderClaimsController(IMapper mapper) : ControllerBase
{
    // [HttpGet]
    // [Route("folder/{folderId:long:min(1)}")]
    // public async Task<ActionResult<GetJobFolderClaimsOverviewResponse>> GetJobFolderClaimsOverview(
    //     [FromRoute] long folderId,
    //     [FromServices] GetJobFolderClaimsOverviewHandler handler,
    //     CancellationToken cancellationToken)
    // {
    //     var request = new GetJobFolderClaimsOverviewRequest(folderId);
    //     
    //     var result = await handler.Handle(request, cancellationToken);
    //     
    //     return this.ToActionResult(result);
    // }
    
    [HttpGet]
    [Route("folder/{folderId:long:min(1)}/user/{userId:long:min(1)}")]
    public async Task<ActionResult<ICollection<long>>> GetJobFolderClaimIdsForUser(
        [FromRoute] long folderId,
        [FromRoute] long userId,
        [FromServices] GetJobFolderClaimIdsForUserHandler handler,
        CancellationToken cancellationToken)
    {
        var mappedQuery = new GetJobFolderClaimIdsForUserQuery(userId, folderId);
        
        var result = await handler.Handle(mappedQuery, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpPut]
    [Route("folder/{folderId:long:min(1)}/user/{userId:long:min(1)}")]
    public async Task<ActionResult> UpdateJobFolderClaimIdsForUser(
        [FromRoute] long folderId,
        [FromRoute] long userId,
        [FromBody] UpdateJobFolderClaimIdsForUserRequest request,
        [FromServices] UpdateJobFolderClaimIdsForUserHandler handler,
        CancellationToken cancellationToken)
    {
        var mappedCommand = mapper.Map<UpdateJobFolderClaimIdsForUserCommand>(request, opt =>
        {
            opt.Items["FolderId"] = folderId;
            opt.Items["UserId"] = userId;
        });
        
        var result = await handler.Handle(mappedCommand, cancellationToken);
        
        return this.ToActionResult(result);
    }
}
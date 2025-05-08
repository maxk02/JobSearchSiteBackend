using Ardalis.Result.AspNetCore;
using AutoMapper;
using Core.Domains.JobFolderClaims.UseCases.GetJobFolderClaimIdsForUser;
using Core.Domains.JobFolderClaims.UseCases.UpdateJobFolderClaimIdsForUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.JobFolderClaims;

[ApiController]
[Route("api/job-folder-claims")]
[Authorize]
public class JobFolderClaimsController(IMapper mapper) : ControllerBase
{
    [HttpGet]
    [Route("/job-folder/{folderId:long:min(1)}/user/{userId:long:min(1)}")]
    public async Task<ActionResult<ICollection<long>>> GetJobFolderClaimIdsForUser(
        [FromRoute] long folderId, [FromRoute] long userId,
        [FromServices] GetJobFolderClaimIdsForUserHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new GetJobFolderClaimIdsForUserRequest(userId, folderId);
        
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpPatch("/job-folder/{folderId:long:min(1)}/user/{userId:long:min(1)}")]
    public async Task<ActionResult> UpdateJobFolderClaimIdsForUser(
        [FromRoute] long folderId, [FromRoute] long userId,
        [FromBody] UpdateJobFolderClaimIdsForUserRequestDto requestDto,
        [FromServices] UpdateJobFolderClaimIdsForUserHandler handler,
        CancellationToken cancellationToken)
    {
        var request = mapper.Map<UpdateJobFolderClaimIdsForUserRequest>(requestDto, opt =>
        {
            opt.Items["FolderId"] = folderId;
            opt.Items["UserId"] = userId;
        });
        
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
}
using Ardalis.Result.AspNetCore;
using Core.Domains.JobApplications.UseCases.AddJobApplication;
using Core.Domains.JobApplications.UseCases.DeleteJobApplication;
using Core.Domains.JobApplications.UseCases.GetApplicationsForJobId;
using Core.Domains.JobApplications.UseCases.UpdateJobApplication;
using Core.Domains.JobApplications.UseCases.UpdateJobApplicationFiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.JobApplications;

[ApiController]
[Route("api/job-applications")]
[Authorize]
public class JobApplicationsController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<AddJobApplicationResponse>> AddJobApplication(
        [FromBody] AddJobApplicationRequest request,
        [FromServices] AddJobApplicationHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpDelete("{id:long:min(1)}")]
    public async Task<ActionResult> DeleteJobApplication(
        long id,
        [FromServices] DeleteJobApplicationHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new DeleteJobApplicationRequest(id);
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpGet]
    public async Task<ActionResult<GetApplicationsForJobIdResponse>> GetApplicationsForJobId(
        [FromQuery] GetApplicationsForJobIdRequest request,
        [FromServices] GetApplicationsForJobIdHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpPatch("{id:long:min(1)}")]
    public async Task<ActionResult> UpdateJobApplication(
        long id,
        [FromBody] UpdateJobApplicationRequest request,
        [FromServices] UpdateJobApplicationHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpPut("{id:long:min(1)}")]
    [Route("/files")]
    public async Task<ActionResult> UpdateJobApplicationFiles(
        long id,
        [FromBody] UpdateJobApplicationFilesRequest request,
        [FromServices] UpdateJobApplicationFilesHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
}
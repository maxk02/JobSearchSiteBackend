using Ardalis.Result.AspNetCore;
using JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.AddJobApplication;
using JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.DeleteJobApplication;
using JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.UpdateJobApplicationFiles;
using JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.UpdateJobApplicationStatus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobSearchSiteBackend.API.Controllers.JobApplications;

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
    
    [HttpDelete]
    [Route("{id:long:min(1)}")]
    public async Task<ActionResult> DeleteJobApplication(
        [FromRoute] long id,
        [FromServices] DeleteJobApplicationHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new DeleteJobApplicationRequest(id);
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpPut]
    [Route("{id:long:min(1)}/files")]
    public async Task<ActionResult> UpdateJobApplicationFiles(
        long id,
        [FromBody] UpdateJobApplicationFilesRequest request,
        [FromServices] UpdateJobApplicationFilesHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpPut]
    [Route("{id:long:min(1)}/status")]
    public async Task<ActionResult> UpdateJobApplicationStatus(
        [FromRoute] long id,
        [FromBody] UpdateJobApplicationStatusRequest request,
        [FromServices] UpdateJobApplicationStatusHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
}
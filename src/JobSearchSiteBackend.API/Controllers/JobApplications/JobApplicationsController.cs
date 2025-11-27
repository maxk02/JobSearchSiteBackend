using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using AutoMapper;
using JobSearchSiteBackend.API.Controllers.JobApplications.Dtos;
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
public class JobApplicationsController(IMapper mapper) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<AddJobApplicationResponse>> AddJobApplication(
        [FromBody] AddJobApplicationRequest request,
        [FromServices] AddJobApplicationHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new AddJobApplicationCommand(request.JobId, request.PersonalFileIds);
        
        var result = await handler.Handle(command, cancellationToken);
        
        return this.ToActionResult(result.Map(x => mapper.Map<AddJobApplicationResponse>(x)));
    }
    
    [HttpDelete]
    [Route("{id:long:min(1)}")]
    public async Task<ActionResult> DeleteJobApplication(
        [FromRoute] long id,
        [FromServices] DeleteJobApplicationHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new DeleteJobApplicationCommand(id);
        
        var result = await handler.Handle(command, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpPut]
    [Route("{id:long:min(1)}/files")]
    public async Task<ActionResult> UpdateJobApplicationFiles(
        [FromRoute] long id,
        [FromBody] UpdateJobApplicationFilesRequest request,
        [FromServices] UpdateJobApplicationFilesHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdateJobApplicationFilesCommand(id, request.PersonalFileIds);
        
        var result = await handler.Handle(command, cancellationToken);
        
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
        var command = new UpdateJobApplicationStatusCommand(id, request.StatusId);
        
        var result = await handler.Handle(command, cancellationToken);
        
        return this.ToActionResult(result);
    }
}
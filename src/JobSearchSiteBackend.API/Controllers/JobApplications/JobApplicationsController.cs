using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using AutoMapper;
using JobSearchSiteBackend.API.Controllers.JobApplications.Dtos;
using JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.AddJobApplication;
using JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.AddJobApplicationTag;
using JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.DeleteJobApplication;
using JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.GetFileDownloadLinkFromJobApplication;
using JobSearchSiteBackend.Core.Domains.JobApplications.UseCases.RemoveJobApplicationTag;
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
        var command = new AddJobApplicationCommand(request.JobId, request.LocationId, request.PersonalFileIds);
        
        var result = await handler.Handle(command, cancellationToken);
        
        return this.ToActionResult(result.Map(x => mapper.Map<AddJobApplicationResponse>(x)));
    }

    [HttpPost]
    [Route("{id:long:min(1)}/tags")]
    public async Task<ActionResult> AddJobApplicationTag(
        [FromRoute] long id,
        [FromBody] AddJobApplicationTagRequest request,
        [FromServices] AddJobApplicationTagHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new AddJobApplicationTagCommand(id, request.Name);
        
        var result = await handler.Handle(command, cancellationToken);
        
        return this.ToActionResult(result);
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

    [HttpGet]
    [Route("{id:long:min(1)}/files/{fileId:long:min(1)}")]
    public async Task<ActionResult<GetFileDownloadLinkFromJobApplicationResponse>> GetFileDownloadLinkFromJobApplication(
        [FromRoute] long id,
        [FromRoute] long fileId,
        [FromServices] GetFileDownloadLinkFromJobApplicationHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetFileDownloadLinkFromJobApplicationQuery(id, fileId);
        
        var result = await handler.Handle(query, cancellationToken);
        
        return this.ToActionResult(result.Map(x => mapper.Map<GetFileDownloadLinkFromJobApplicationResponse>(x)));
    }

    [HttpDelete]
    [Route("{id:long:min(1)}/tags/{tag:minLength(1)}")]
    public async Task<ActionResult> RemoveJobApplication(
        [FromRoute] long id,
        [FromRoute] string tag,
        [FromServices] RemoveJobApplicationTagHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new RemoveJobApplicationTagCommand(id, tag);
        
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
        var command = new UpdateJobApplicationFilesCommand(id, request.LocationId, request.PersonalFileIds);
        
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
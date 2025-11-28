using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using AutoMapper;
using JobSearchSiteBackend.API.Controllers.JobFolders.Dtos;
using JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.AddJobFolder;
using JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.DeleteJobFolder;
using JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.GetChildFolders;
using JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.GetFolderJobs;
using JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.GetJobFolder;
using JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.UpdateJobFolder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobSearchSiteBackend.API.Controllers.JobFolders;

[ApiController]
[Route("api/job-folders")]
[Authorize]
public class JobFoldersController(IMapper mapper) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<long>> AddJobFolder(
        [FromBody] AddJobFolderRequest request,
        [FromServices] AddJobFolderHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new AddJobFolderCommand(request.CompanyId, request.ParentId,
            request.Name, request.Description);
        
        var result = await handler.Handle(command, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpDelete]
    [Route("{id:long:min(1)}")]
    public async Task<ActionResult> DeleteJobFolder(
        [FromRoute] long id,
        [FromServices] DeleteJobFolderHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new DeleteJobFolderCommand(id);
        
        var result = await handler.Handle(command, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpGet]
    [Route("{id:long:min(1)}/child-folders")]
    public async Task<ActionResult<GetChildFoldersResponse>> GetChildFolders(
        [FromRoute] long id,
        [FromServices] GetChildFoldersHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetChildFoldersQuery(id);
        
        var result = await handler.Handle(query, cancellationToken);
        
        return this.ToActionResult(result.Map(x => mapper.Map<GetChildFoldersResponse>(x)));
    }
    
    [HttpGet]
    [Route("{id:long:min(1)}")]
    public async Task<ActionResult<GetJobFolderResponse>> GetJobFolder(
        [FromRoute] long id,
        [FromServices] GetJobFolderHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetJobFolderQuery(id);
        
        var result = await handler.Handle(query, cancellationToken);
        
        return this.ToActionResult(result.Map(x => mapper.Map<GetJobFolderResponse>(x)));
    }
    
    [HttpGet]
    [Route("{id:long:min(1)}/jobs")]
    public async Task<ActionResult<GetFolderJobsResponse>> GetJobs(
        [FromRoute] long id,
        [FromServices] GetFolderJobsHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetFolderJobsQuery(id);
        
        var result = await handler.Handle(query, cancellationToken);
        
        return this.ToActionResult(result.Map(x => mapper.Map<GetFolderJobsResponse>(x)));
    }
    
    [HttpPatch]
    [Route("{id:long:min(1)}")]
    public async Task<ActionResult> UpdateJobFolder(
        [FromRoute] long id,
        [FromBody] UpdateJobFolderRequest request,
        [FromServices] UpdateJobFolderHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdateJobFolderCommand(id, request.Name, request.Description);
        
        var result = await handler.Handle(command, cancellationToken);
        
        return this.ToActionResult(result);
    }
}
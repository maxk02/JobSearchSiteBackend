using Ardalis.Result.AspNetCore;
using Core.Domains.JobFolders.UseCases.AddJobFolder;
using Core.Domains.JobFolders.UseCases.DeleteJobFolder;
using Core.Domains.JobFolders.UseCases.GetChildFolders;
using Core.Domains.JobFolders.UseCases.GetJobs;
using Core.Domains.JobFolders.UseCases.UpdateJobFolder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class JobFoldersController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<long>> CreateJobFolder(
        [FromBody] AddJobFolderRequest request,
        [FromServices] AddJobFolderHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpDelete("{id:long:min(1)}")]
    public async Task<ActionResult> DeleteJobFolder(
        long id,
        [FromServices] DeleteJobFolderHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new DeleteJobFolderRequest(id);
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpGet("{id:long:min(1)}/jobs")]
    public async Task<ActionResult<GetJobsResponse>> GetJobs(
        long id,
        [FromServices] GetJobsHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new GetJobsRequest(id);
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpGet("{id:long:min(1)}/child-folders")]
    public async Task<ActionResult<GetChildFoldersResponse>> GetChildFolders(
        long id,
        [FromServices] GetChildFoldersHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new GetChildFoldersRequest(id);
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpPatch]
    public async Task<ActionResult> UpdateJobFolder(
        [FromBody] UpdateJobFolderRequest request,
        [FromServices] UpdateJobFolderHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
}
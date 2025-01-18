using Ardalis.Result.AspNetCore;
using Core.Domains.Jobs.UseCases.AddJob;
using Core.Domains.Jobs.UseCases.DeleteJob;
using Core.Domains.Jobs.UseCases.GetJobs;
using Core.Domains.Jobs.UseCases.GetJobById;
using Core.Domains.Jobs.UseCases.UpdateJob;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class JobsController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> AddJob(
        [FromBody] AddJobRequest request,
        [FromServices] AddJobHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpDelete("{id:long:min(1)}")]
    public async Task<ActionResult> DeleteJob(
        long id,
        [FromServices] DeleteJobHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new DeleteJobRequest(id);
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<GetJobsResponse>> GetJobs(
        [FromQuery] GetJobsRequest request,
        [FromServices] GetJobsHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpGet("{id:long:min(1)}")]
    [AllowAnonymous]
    public async Task<ActionResult<GetJobByIdResponse>> GetJob(
        long id,
        [FromServices] GetJobByIdHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new GetJobByIdRequest(id);
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpPatch]
    public async Task<ActionResult> UpdateJob(
        [FromBody] UpdateJobRequest request,
        [FromServices] UpdateJobHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
}
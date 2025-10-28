using Ardalis.Result.AspNetCore;
using AutoMapper;
using JobSearchSiteBackend.Core.Domains.Jobs.UseCases.AddJob;
using JobSearchSiteBackend.Core.Domains.Jobs.UseCases.DeleteJob;
using JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetApplicationsForJob;
using JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetJob;
using JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetJobManagementDto;
using JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetJobs;
using JobSearchSiteBackend.Core.Domains.Jobs.UseCases.UpdateJob;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobSearchSiteBackend.API.Controllers.Jobs;

[ApiController]
[Route("api/jobs")]
[Authorize]
public class JobsController(IMapper mapper) : ControllerBase
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
    
    [HttpDelete]
    [Route("{id:long:min(1)}")]
    public async Task<ActionResult> DeleteJob(
        [FromRoute] long id,
        [FromServices] DeleteJobHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new DeleteJobRequest(id);
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<GetApplicationsForJobResponse>> GetApplicationsForJob(
        [FromQuery] GetApplicationsForJobRequest request,
        [FromServices] GetApplicationsForJobHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpGet]
    [Route("{id:long:min(1)}")]
    [AllowAnonymous]
    public async Task<ActionResult<GetJobResponse>> GetJob(
        [FromRoute] long id,
        [FromServices] GetJobHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new GetJobRequest(id);
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpGet]
    [Route("{id:long:min(1)}/management")]
    public async Task<ActionResult<GetJobManagementDtoResponse>> GetJobManagementDto(
        [FromRoute] long id,
        [FromServices] GetJobManagementDtoHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new GetJobManagementDtoRequest(id);
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
    
    [HttpPatch]
    [Route("{id:long:min(1)}")]
    public async Task<ActionResult> UpdateJob(
        [FromRoute] long id,
        [FromBody] UpdateJobRequestDto requestDto,
        [FromServices] UpdateJobHandler handler,
        CancellationToken cancellationToken)
    {
        var request = mapper.Map<UpdateJobRequest>(requestDto, opt =>
        {
            opt.Items["Id"] = id;
        });
        
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
}
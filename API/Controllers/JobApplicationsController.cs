using Core.Domains.JobApplications.UseCases.AddJobApplication;
using Core.Domains.JobApplications.UseCases.DeleteJobApplication;
using Core.Domains.JobApplications.UseCases.GetApplicationsForJobId;
using Core.Domains.JobApplications.UseCases.UpdateJobApplication;
using Core.Domains.JobApplications.UseCases.UpdateJobApplicationFiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class JobApplicationsController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> AddJobApplication(
        [FromBody] AddJobApplicationRequest request,
        [FromServices] AddJobApplicationHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        throw new NotImplementedException();
    }
    
    [HttpDelete("{id:long:min(1)}")]
    public async Task<ActionResult> DeleteJobApplication(
        long id,
        [FromServices] DeleteJobApplicationHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new DeleteJobApplicationRequest(id);
        var result = await handler.Handle(request, cancellationToken);
        
        throw new NotImplementedException();
    }
    
    [HttpGet]
    public async Task<ActionResult> GetApplicationsForJobId(
        [FromQuery] GetApplicationsForJobIdRequest request,
        [FromServices] GetApplicationsForJobIdHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        throw new NotImplementedException();
    }
    
    [HttpPatch]
    public async Task<ActionResult> UpdateJobApplication(
        [FromBody] UpdateJobApplicationRequest request,
        [FromServices] UpdateJobApplicationHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        throw new NotImplementedException();
    }
    
    [HttpPut]
    [Route("/files")]
    public async Task<ActionResult> UpdateJobApplicationFiles(
        [FromBody] UpdateJobApplicationFilesRequest request,
        [FromServices] UpdateJobApplicationFilesHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        throw new NotImplementedException();
    }
}
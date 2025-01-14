using Core.Domains.JobFolders.UseCases.AddJobFolder;
using Core.Domains.JobFolders.UseCases.DeleteJobFolder;
using Core.Domains.JobFolders.UseCases.GetChildJobsAndFolders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class JobFoldersController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> CreateJobFolder(
        [FromBody] AddJobFolderRequest request,
        [FromServices] AddJobFolderHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        throw new NotImplementedException();
    }
    
    [HttpDelete("{id:long:min(1)}")]
    public async Task<ActionResult> DeleteJobFolder(
        long id,
        [FromServices] DeleteJobFolderHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new DeleteJobFolderRequest(id);
        var result = await handler.Handle(request, cancellationToken);
        
        throw new NotImplementedException();
    }
    
    [HttpGet("{id:long:min(1)}")]
    public async Task<ActionResult> GetChildJobsAndFolders(
        long id,
        [FromServices] GetChildJobsAndFoldersHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new GetChildJobsAndFoldersRequest(id);
        var result = await handler.Handle(request, cancellationToken);
        
        throw new NotImplementedException();
    }
}
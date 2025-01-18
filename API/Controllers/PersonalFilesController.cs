using Ardalis.Result.AspNetCore;
using Core.Domains.PersonalFiles.UseCases.DeleteFile;
using Core.Domains.PersonalFiles.UseCases.UpdateFile;
using Core.Domains.PersonalFiles.UseCases.UploadFile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PersonalFilesController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> UploadFile(
        [FromBody] UploadFileRequest request,
        [FromServices] UploadFileHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpDelete("{id:long:min(1)}")]
    public async Task<ActionResult> DeleteFile(
        long id,
        [FromServices] DeleteFileHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new DeleteFileRequest(id);
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpPatch]
    public async Task<ActionResult> UpdateFile(
        [FromBody] UpdateFileRequest request,
        [FromServices] UpdateFileHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
}
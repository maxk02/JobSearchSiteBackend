using Ardalis.Result.AspNetCore;
using AutoMapper;
using Core.Domains.PersonalFiles.UseCases.DeleteFile;
using Core.Domains.PersonalFiles.UseCases.UpdateFile;
using Core.Domains.PersonalFiles.UseCases.UploadFile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.PersonalFiles;

[ApiController]
[Route("api/personal-files")]
[Authorize]
public class PersonalFilesController(IMapper mapper) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> UploadFile(
        IFormFile? file,
        [FromServices] UploadFileHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new UploadFileRequest(file);
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
    
    [HttpPatch("{id:long:min(1)}")]
    public async Task<ActionResult> UpdateFile(
        long id,
        [FromBody] UpdateFileRequestDto requestDto,
        [FromServices] UpdateFileHandler handler,
        CancellationToken cancellationToken)
    {
        var request = mapper.Map<UpdateFileRequest>(requestDto, opt =>
        {
            opt.Items["Id"] = id;
        });
        
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
}
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using AutoMapper;
using JobSearchSiteBackend.Core.Domains.PersonalFiles.UseCases.DeleteFile;
using JobSearchSiteBackend.Core.Domains.PersonalFiles.UseCases.GetDownloadLink;
using JobSearchSiteBackend.Core.Domains.PersonalFiles.UseCases.UpdateFile;
using JobSearchSiteBackend.Core.Domains.PersonalFiles.UseCases.UploadFile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobSearchSiteBackend.API.Controllers.PersonalFiles;

[ApiController]
[Route("api/personal-files")]
[Authorize]
public class PersonalFilesController(IMapper mapper) : ControllerBase
{
    [HttpDelete("{id:long:min(1)}")]
    public async Task<ActionResult> DeleteFile(
        [FromRoute] long id,
        [FromServices] DeleteFileHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new DeleteFileRequest(id);
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpGet]
    [Route("{id:long:min(1)}/download-link")]
    public async Task<ActionResult<GetDownloadLinkResponse>> GetDownloadLink(
        [FromRoute] long id,
        [FromServices] GetDownloadLinkHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new GetDownloadLinkRequest(id);
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpPatch("{id:long:min(1)}")]
    public async Task<ActionResult> UpdateFile(
        [FromRoute] long id,
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
    
    [HttpPost]
    public async Task<ActionResult<UploadFileResponse>> UploadFile(
        [FromForm] IFormFile file,
        [FromServices] UploadFileHandler handler,
        CancellationToken cancellationToken)
    {
        if (file.Length == 0)
        {
            return this.ToActionResult(Result.Invalid());
        }

        await using var stream = file.OpenReadStream();
        
        var name = Path.GetFileNameWithoutExtension(file.FileName);
        var extension = Path.GetExtension(file.FileName).TrimStart('.');
        var size = file.Length;
        
        var request = new UploadFileRequest(stream, name, extension, size);
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
}
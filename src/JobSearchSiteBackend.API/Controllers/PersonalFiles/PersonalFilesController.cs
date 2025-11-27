using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using AutoMapper;
using JobSearchSiteBackend.API.Controllers.PersonalFiles.Dtos;
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
        var mappedCommand = new DeleteFileCommand(id);
        
        var result = await handler.Handle(mappedCommand, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpGet]
    [Route("{id:long:min(1)}/download-link")]
    public async Task<ActionResult<GetDownloadLinkResponse>> GetDownloadLink(
        [FromRoute] long id,
        [FromServices] GetDownloadLinkHandler handler,
        CancellationToken cancellationToken)
    {
        var mappedQuery = new GetDownloadLinkQuery(id);
        
        var result = await handler.Handle(mappedQuery, cancellationToken);
        
        return this.ToActionResult(result.Map(x => mapper.Map<GetDownloadLinkResponse>(x)));
    }
    
    [HttpPatch("{id:long:min(1)}")]
    public async Task<ActionResult> UpdateFile(
        [FromRoute] long id,
        [FromBody] UpdateFileRequest request,
        [FromServices] UpdateFileHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdateFileCommand(id, request.Name);
        
        var result = await handler.Handle(command, cancellationToken);
        
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
        
        var request = new UploadFileCommand(stream, name, extension, size);
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result.Map(x => mapper.Map<UploadFileResponse>(x)));
    }
}
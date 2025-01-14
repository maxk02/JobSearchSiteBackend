using Core.Domains.Cvs.UseCases.AddCv;
using Core.Domains.Cvs.UseCases.DeleteCv;
using Core.Domains.Cvs.UseCases.GetCvById;
using Core.Domains.Cvs.UseCases.UpdateCv;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CvsController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> AddCv(
        [FromBody] AddCvRequest request,
        [FromServices] AddCvHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        throw new NotImplementedException();
    }
    
    [HttpDelete("{id:long:min(1)}")]
    public async Task<ActionResult> DeleteCv(
        long id,
        [FromServices] DeleteCvHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new DeleteCvRequest(id);
        var result = await handler.Handle(request, cancellationToken);
        
        throw new NotImplementedException();
    }
    
    [HttpGet("{id:long:min(1)}")]
    public async Task<ActionResult> GetCv(
        long id,
        [FromServices] GetCvByIdHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new GetCvByIdRequest(id);
        var result = await handler.Handle(request, cancellationToken);
        
        throw new NotImplementedException();
    }
    
    [HttpPatch]
    public async Task<ActionResult> UpdateCv(
        [FromBody] UpdateCvRequest request,
        [FromServices] UpdateCvHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        throw new NotImplementedException();
    }
}
using Core.Domains.Companies.UseCases.AddCompany;
using Core.Domains.Companies.UseCases.DeleteCompany;
using Core.Domains.Companies.UseCases.GetCompanies;
using Core.Domains.Companies.UseCases.GetCompanyById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CompaniesController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> AddCompany(
        [FromBody] AddCompanyRequest request,
        [FromServices] AddCompanyHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        throw new NotImplementedException();
    }
    
    [HttpDelete("{id:long:min(1)}")]
    public async Task<ActionResult> DeleteCompany(
        long id,
        [FromServices] DeleteCompanyHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new DeleteCompanyRequest(id);
        var result = await handler.Handle(request, cancellationToken);
        
        throw new NotImplementedException();
    }
    
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult> GetCompanies(
        [FromQuery] GetCompaniesRequest request,
        [FromServices] GetCompaniesHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        throw new NotImplementedException();
    }
    
    [HttpGet("{id:long:min(1)}")]
    [AllowAnonymous]
    public async Task<ActionResult> GetCompany(
        long id,
        [FromServices] GetCompanyByIdHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new GetCompanyByIdRequest(id);
        var result = await handler.Handle(request, cancellationToken);
        
        throw new NotImplementedException();
    }
}
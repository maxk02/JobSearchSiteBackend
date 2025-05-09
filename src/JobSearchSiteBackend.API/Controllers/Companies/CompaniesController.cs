using Ardalis.Result.AspNetCore;
using AutoMapper;
using Core.Domains.Companies.UseCases.AddCompany;
using Core.Domains.Companies.UseCases.DeleteCompany;
using Core.Domains.Companies.UseCases.GetCompanies;
using Core.Domains.Companies.UseCases.GetCompanyById;
using Core.Domains.Companies.UseCases.UpdateCompany;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobSearchSiteBackend.API.Controllers.Companies;

[ApiController]
[Route("api/companies")]
[Authorize]
public class CompaniesController(IMapper mapper) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<AddCompanyResponse>> AddCompany(
        [FromBody] AddCompanyRequest request,
        [FromServices] AddCompanyHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);

        return this.ToActionResult(result);
    }

    [HttpDelete("{id:long:min(1)}")]
    public async Task<ActionResult> DeleteCompany(
        [FromRoute] long id,
        [FromServices] DeleteCompanyHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new DeleteCompanyRequest(id);
        var result = await handler.Handle(request, cancellationToken);

        return this.ToActionResult(result);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<GetCompaniesResponse>> GetCompanies(
        [FromQuery] GetCompaniesRequest request,
        [FromServices] GetCompaniesHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);

        return this.ToActionResult(result);
    }

    [HttpGet("{id:long:min(1)}")]
    [AllowAnonymous]
    public async Task<ActionResult<GetCompanyByIdResponse>> GetCompany(
        [FromRoute] long id,
        [FromServices] GetCompanyByIdHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new GetCompanyByIdRequest(id);
        var result = await handler.Handle(request, cancellationToken);

        return this.ToActionResult(result);
    }

    [HttpPatch("{id:long:min(1)}")]
    public async Task<ActionResult> UpdateCompany(
        [FromRoute] long id,
        [FromBody] UpdateCompanyRequestDto requestDto,
        [FromServices] UpdateCompanyHandler handler,
        CancellationToken cancellationToken)
    {
        var request = mapper.Map<UpdateCompanyRequest>(requestDto, opt =>
        {
            opt.Items["Id"] = id;
        });
        
        var result = await handler.Handle(request, cancellationToken);

        return this.ToActionResult(result);
    }
}
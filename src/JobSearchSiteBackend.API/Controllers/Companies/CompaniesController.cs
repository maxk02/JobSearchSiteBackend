using Ardalis.Result.AspNetCore;
using AutoMapper;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.AddCompany;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.DeleteCompany;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompany;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyLastVisitedFolders;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyLastVisitedJobs;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.UpdateCompany;
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
        [FromForm] AddCompanyRequest request,
        [FromForm] IFormFile avatarFile,
        [FromServices] AddCompanyHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);

        return this.ToActionResult(result);
    }
    
    // [HttpPost]
    // [Route("/{companyId:long:min(1)}/management/users")]
    // public async Task<ActionResult<AddCompanyResponse>> AddCompanyEmployee(
    //     [FromRoute] long companyId,
    //     [FromBody] AddCompanyEmployeeRequest request,
    //     [FromServices] AddCompanyEmployeeHandler handler,
    //     CancellationToken cancellationToken)
    // {
    //     var result = await handler.Handle(request, cancellationToken);
    //
    //     return this.ToActionResult(result);
    // }

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

    // [HttpGet]
    // [AllowAnonymous]
    // public async Task<ActionResult<GetCompaniesResponse>> GetCompanies(
    //     [FromQuery] GetCompaniesRequest request,
    //     [FromServices] GetCompaniesHandler handler,
    //     CancellationToken cancellationToken)
    // {
    //     var result = await handler.Handle(request, cancellationToken);
    //
    //     return this.ToActionResult(result);
    // }

    [HttpGet("{id:long:min(1)}")]
    [AllowAnonymous]
    public async Task<ActionResult<GetCompanyResponse>> GetCompany(
        [FromRoute] long id,
        [FromServices] GetCompanyHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new GetCompanyRequest(id);
        var result = await handler.Handle(request, cancellationToken);

        return this.ToActionResult(result);
    }
    
    // [HttpGet]
    // [Route("/{id:long:min(1)}/management/users")]
    // public async Task<ActionResult<GetCompanyEmployeesResponse>> GetCompanyEmployees(
    //     [FromRoute] long id,
    //     [FromServices] GetCompanyEmployeesHandler handler,
    //     CancellationToken cancellationToken)
    // {
    //     var request = new GetCompanyEmployeesRequest(id);
    //     var result = await handler.Handle(request, cancellationToken);
    //
    //     return this.ToActionResult(result);
    // }
    //
    // [HttpGet]
    // [Route("/{id:long:min(1)}/jobs")]
    // [AllowAnonymous]
    // public async Task<ActionResult<GetCompanyJobsResponse>> GetCompanyJobs(
    //     [FromRoute] long id,
    //     [FromServices] GetCompanyJobsHandler handler,
    //     CancellationToken cancellationToken)
    // {
    //     var request = new GetCompanyJobsRequest(id);
    //     var result = await handler.Handle(request, cancellationToken);
    //
    //     return this.ToActionResult(result);
    // }
    //
    // [HttpGet]
    // [Route("/{id:long:min(1)}/management/last-visited-folders")]
    // public async Task<ActionResult<GetCompanyLastVisitedFoldersResponse>> GetCompanyLastVisitedFolders(
    //     [FromRoute] long id,
    //     [FromServices] GetCompanyLastVisitedFoldersHandler handler,
    //     CancellationToken cancellationToken)
    // {
    //     var request = new GetCompanyLastVisitedFoldersRequest(id);
    //     var result = await handler.Handle(request, cancellationToken);
    //
    //     return this.ToActionResult(result);
    // }
    //
    // [HttpGet]
    // [Route("/{id:long:min(1)}/management/last-visited-jobs")]
    // public async Task<ActionResult<GetCompanyLastVisitedJobsResponse>> GetCompanyLastVisitedJobs(
    //     [FromRoute] long id,
    //     [FromServices] GetCompanyLastVisitedJobsHandler handler,
    //     CancellationToken cancellationToken)
    // {
    //     var request = new GetCompanyLastVisitedJobsRequest(id);
    //     var result = await handler.Handle(request, cancellationToken);
    //
    //     return this.ToActionResult(result);
    // }
    //
    // [HttpGet]
    // [Route("/{id:long:min(1)}/management")]
    // public async Task<ActionResult<GetCompanyManagementNavbarResponse>> GetCompanyManagementNavbarDto(
    //     [FromRoute] long id,
    //     [FromServices] GetCompanyManagementNavbarHandler handler,
    //     CancellationToken cancellationToken)
    // {
    //     var request = new GetCompanyManagementNavbarRequest(id);
    //     var result = await handler.Handle(request, cancellationToken);
    //
    //     return this.ToActionResult(result);
    // }
    //
    // [HttpGet]
    // [Route("/{id:long:min(1)}/management/job-folders")]
    // public async Task<ActionResult<GetCompanySharedFoldersResponse>> GetCompanySharedFolders(
    //     [FromRoute] long id,
    //     [FromServices] GetCompanySharedFoldersHandler handler,
    //     CancellationToken cancellationToken)
    // {
    //     var request = new GetCompanySharedFoldersRequest(id);
    //     var result = await handler.Handle(request, cancellationToken);
    //
    //     return this.ToActionResult(result);
    // }
    //
    // [HttpGet]
    // [Route("/{id:long:min(1)}/management/job-folders/{parentFolderId:long:min(1)}")]
    // public async Task<ActionResult<GetCompanySharedFoldersResponse>> GetCompanySharedFoldersChildren(
    //     [FromRoute] long id,
    //     [FromRoute] long parentFolderId,
    //     [FromServices] GetCompanySharedFoldersHandler handler,
    //     CancellationToken cancellationToken)
    // {
    //     var request = new GetCompanySharedFoldersRequest(id, parentFolderId);
    //     var result = await handler.Handle(request, cancellationToken);
    //
    //     return this.ToActionResult(result);
    // }
    //
    // [HttpDelete]
    // [Route("/{id:long:min(1)}/management/last-visited-folders")]
    // public async Task<ActionResult> RemoveCompanyAllLastVisitedFolders(
    //     [FromRoute] long id,
    //     [FromServices] RemoveCompanyLastVisitedFoldersHandler handler,
    //     CancellationToken cancellationToken)
    // {
    //     var request = new RemoveCompanyLastVisitedFoldersRequest(id);
    //     var result = await handler.Handle(request, cancellationToken);
    //
    //     return this.ToActionResult(result);
    // }
    //
    // [HttpDelete]
    // [Route("/{id:long:min(1)}/management/last-visited-folders/{folderId:long:min(1)}")]
    // public async Task<ActionResult> RemoveCompanyLastVisitedFolder(
    //     [FromRoute] long id,
    //     [FromRoute] long folderId,
    //     [FromServices] RemoveCompanyLastVisitedFoldersHandler handler,
    //     CancellationToken cancellationToken)
    // {
    //     var request = new RemoveCompanyLastVisitedFoldersRequest(id, folderId);
    //     var result = await handler.Handle(request, cancellationToken);
    //
    //     return this.ToActionResult(result);
    // }
    //
    // [HttpDelete]
    // [Route("/{id:long:min(1)}/management/last-visited-jobs")]
    // public async Task<ActionResult> RemoveCompanyAllLastVisitedJobs(
    //     [FromRoute] long id,
    //     [FromServices] RemoveCompanyLastVisitedJobsHandler handler,
    //     CancellationToken cancellationToken)
    // {
    //     var request = new RemoveCompanyLastVisitedJobsRequest(id);
    //     var result = await handler.Handle(request, cancellationToken);
    //
    //     return this.ToActionResult(result);
    // }
    //
    // [HttpDelete]
    // [Route("/{id:long:min(1)}/management/last-visited-jobs/{jobId:long:min(1)}")]
    // public async Task<ActionResult> RemoveCompanyLastVisitedJob(
    //     [FromRoute] long id,
    //     [FromRoute] long jobId,
    //     [FromServices] RemoveCompanyLastVisitedJobsHandler handler,
    //     CancellationToken cancellationToken)
    // {
    //     var request = new RemoveCompanyLastVisitedJobsRequest(id, jobId);
    //     var result = await handler.Handle(request, cancellationToken);
    //
    //     return this.ToActionResult(result);
    // }
    //
    // [HttpDelete]
    // [Route("/{companyId:long:min(1)}/management/users/{userId:long:min(1)}")]
    // public async Task<ActionResult> RemoveCompanyEmployee(
    //     [FromRoute] long companyId,
    //     [FromRoute] long userId,
    //     [FromServices] RemoveCompanyEmployeeHandler handler,
    //     CancellationToken cancellationToken)
    // {
    //     var result = await handler.Handle(request, cancellationToken);
    //
    //     return this.ToActionResult(result);
    // }
    //
    // [HttpGet]
    // [Route("/{id:long:min(1)}/management/job-folders/search")]
    // public async Task<ActionResult<SearchCompanySharedFoldersResponse>> SearchCompanySharedFolders(
    //     [FromRoute] long id,
    //     [FromServices] SearchCompanySharedFoldersHandler handler,
    //     CancellationToken cancellationToken)
    // {
    //     var request = new SearchCompanySharedFoldersRequest(id);
    //     var result = await handler.Handle(request, cancellationToken);
    //
    //     return this.ToActionResult(result);
    // }
    //
    // [HttpGet]
    // [Route("/{id:long:min(1)}/management/jobs/search")]
    // public async Task<ActionResult<SearchCompanySharedJobsResponse>> SearchCompanySharedJobs(
    //     [FromRoute] long id,
    //     [FromServices] SearchCompanySharedJobsHandler handler,
    //     CancellationToken cancellationToken)
    // {
    //     var request = new SearchCompanySharedJobsRequest(id);
    //     var result = await handler.Handle(request, cancellationToken);
    //
    //     return this.ToActionResult(result);
    // }

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
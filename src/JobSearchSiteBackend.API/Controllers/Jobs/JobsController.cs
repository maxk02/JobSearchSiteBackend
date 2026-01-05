using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using AutoMapper;
using JobSearchSiteBackend.API.Controllers.Jobs.Dtos;
using JobSearchSiteBackend.Core.Domains.Jobs.UseCases.AddJob;
using JobSearchSiteBackend.Core.Domains.Jobs.UseCases.DeleteJob;
using JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetApplicationsForJob;
using JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetJob;
using JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetJobManagementDto;
using JobSearchSiteBackend.Core.Domains.Jobs.UseCases.GetJobs;
using JobSearchSiteBackend.Core.Domains.Jobs.UseCases.UpdateJob;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobSearchSiteBackend.API.Controllers.Jobs;

[ApiController]
[Route("api/jobs")]
[Authorize]
public class JobsController(IMapper mapper) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<AddJobResponse>> AddJob(
        [FromBody] AddJobRequest request,
        [FromServices] AddJobHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new AddJobCommand(request.CompanyId, request.CategoryId, request.Title, request.Description,
            request.IsPublic, request.DateTimeExpiringUtc, request.Responsibilities, request.Requirements,
            request.NiceToHaves, request.JobSalaryInfoDto, request.EmploymentOptionIds, request.ContractTypeIds,
            request.LocationIds);

        var result = await handler.Handle(command, cancellationToken);

        return this.ToActionResult(result.Map(x => mapper.Map<AddJobResponse>(x)));
    }

    [HttpDelete]
    [Route("{id:long:min(1)}")]
    public async Task<ActionResult> DeleteJob(
        [FromRoute] long id,
        [FromServices] DeleteJobHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new DeleteJobCommand(id);

        var result = await handler.Handle(command, cancellationToken);

        return this.ToActionResult(result);
    }

    [HttpGet]
    [Route("{id:long:min(1)}/applications")]
    public async Task<ActionResult<GetApplicationsForJobResponse>> GetApplicationsForJob(
        [FromRoute] long id,
        [FromQuery] GetApplicationsForJobRequest request,
        [FromServices] GetApplicationsForJobHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetApplicationsForJobQuery(id, request.StatusIds, request.Query, request.SortOption,
            request.IncludedTags, request.ExcludedTags, request.Page, request.Size);

        var result = await handler.Handle(query, cancellationToken);

        return this.ToActionResult(result.Map(x => mapper.Map<GetApplicationsForJobResponse>(x)));
    }

    [HttpGet]
    [Route("{id:long:min(1)}")]
    [AllowAnonymous]
    public async Task<ActionResult<GetJobResponse>> GetJob(
        [FromRoute] long id,
        [FromServices] GetJobHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetJobQuery(id);

        var result = await handler.Handle(query, cancellationToken);

        return this.ToActionResult(result.Map(x => mapper.Map<GetJobResponse>(x)));
    }

    [HttpGet]
    [Route("{id:long:min(1)}/management")]
    public async Task<ActionResult<GetJobManagementDtoResponse>> GetJobManagementDto(
        [FromRoute] long id,
        [FromServices] GetJobManagementDtoHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetJobManagementDtoQuery(id);

        var result = await handler.Handle(query, cancellationToken);

        return this.ToActionResult(result.Map(x => mapper.Map<GetJobManagementDtoResponse>(x)));
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<GetJobsResponse>> GetJobs(
        [FromQuery] GetJobsRequest request,
        [FromServices] GetJobsHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetJobsQuery(request.Query, request.Page, request.Size, request.MustHaveSalaryRecord,
            request.EmploymentOptionIds, null, request.CategoryIds, request.ContractTypeIds);

        var result = await handler.Handle(query, cancellationToken);

        return this.ToActionResult(result.Map(x => mapper.Map<GetJobsResponse>(x)));
    }

    [HttpPatch]
    [Route("{id:long:min(1)}")]
    public async Task<ActionResult> UpdateJob(
        [FromRoute] long id,
        [FromBody] UpdateJobRequest request,
        [FromServices] UpdateJobHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdateJobCommand(id, request.CategoryId, request.Title,
            request.Description, request.IsPublic, request.DateTimeExpiringUtc, request.Responsibilities,
            request.Requirements, request.NiceToHaves, request.SalaryInfo, request.EmploymentOptionIds,
            request.ContractTypeIds, request.LocationIds);

        var result = await handler.Handle(command, cancellationToken);

        return this.ToActionResult(result);
    }
}
﻿using Ardalis.Result.AspNetCore;
using AutoMapper;
using JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.AddJobFolder;
using JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.DeleteJobFolder;
using JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.GetChildFolders;
using JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.GetJobs;
using JobSearchSiteBackend.Core.Domains.JobFolders.UseCases.UpdateJobFolder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobSearchSiteBackend.API.Controllers.JobFolders;

[ApiController]
[Route("api/job-folders")]
[Authorize]
public class JobFoldersController(IMapper mapper) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<long>> CreateJobFolder(
        [FromBody] AddJobFolderRequest request,
        [FromServices] AddJobFolderHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpDelete("{id:long:min(1)}")]
    public async Task<ActionResult> DeleteJobFolder(
        long id,
        [FromServices] DeleteJobFolderHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new DeleteJobFolderRequest(id);
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpGet("{id:long:min(1)}/jobs")]
    public async Task<ActionResult<GetJobsResponse>> GetJobs(
        long id,
        [FromServices] GetJobsHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new GetJobsRequest(id);
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpGet("{id:long:min(1)}/child-folders")]
    public async Task<ActionResult<GetChildFoldersResponse>> GetChildFolders(
        long id,
        [FromServices] GetChildFoldersHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new GetChildFoldersRequest(id);
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpPatch("{id:long:min(1)}")]
    public async Task<ActionResult> UpdateJobFolder(
        long id,
        [FromBody] UpdateJobFolderRequestDto requestDto,
        [FromServices] UpdateJobFolderHandler handler,
        CancellationToken cancellationToken)
    {
        var request = mapper.Map<UpdateJobFolderRequest>(requestDto, opt =>
        {
            opt.Items["Id"] = id;
        });
        
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
}
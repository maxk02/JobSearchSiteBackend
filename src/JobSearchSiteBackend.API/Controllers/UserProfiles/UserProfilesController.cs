using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using AutoMapper;
using JobSearchSiteBackend.API.Attributes;
using JobSearchSiteBackend.API.Controllers.UserProfiles.Dtos;
using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
using JobSearchSiteBackend.Core.Domains.Accounts.UseCases.GetAccountData;
using JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.AddJobBookmark;
using JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.AddUserProfile;
using JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.DeleteJobBookmark;
using JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.GetBookmarkedJobs;
using JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.GetJobApplications;
using JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.GetPersonalFiles;
using JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.GetUserProfile;
using JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.UpdateUserProfile;
using JobSearchSiteBackend.Core.Domains.UserProfiles.UseCases.UploadUserAvatar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobSearchSiteBackend.API.Controllers.UserProfiles;

[ApiController]
[Route("api/users")]
[Authorize]
public class UserProfilesController(IMapper mapper) : ControllerBase
{
    [HttpPost]
    [Route("current/bookmarks/jobs/{jobId:long:min(1)}")]
    public async Task<ActionResult> AddJobBookmark(
        [FromRoute] long jobId,
        [FromServices] AddJobBookmarkHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new AddJobBookmarkCommand(jobId);
        
        var result = await handler.Handle(command, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpPost]
    public async Task<ActionResult<AddUserProfileResponse>> AddUserProfile(
        [FromBody] AddUserProfileRequest request,
        [FromServices] AddUserProfileHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new AddUserProfileCommand(request.FirstName, request.LastName, request.Phone);
        
        var result = await handler.Handle(command, cancellationToken);
        
        return this.ToActionResult(result.Map(x => mapper.Map<AddUserProfileResponse>(x)));
    }
    
    [HttpDelete]
    [Route("current/bookmarks/jobs/{jobId:long:min(1)}")]
    public async Task<ActionResult> DeleteJobBookmark(
        [FromRoute] long jobId,
        [FromServices] DeleteJobBookmarkHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new DeleteJobBookmarkCommand(jobId);
        
        var result = await handler.Handle(command, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpGet]
    [Route("current/bookmarks/jobs")]
    public async Task<ActionResult<GetBookmarkedJobsResponse>> GetBookmarkedJobs(
        [FromQuery] GetBookmarkedJobsRequest request,
        [FromServices] GetBookmarkedJobsHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetBookmarkedJobsQuery(request.Page, request.Size);
        
        var result = await handler.Handle(query, cancellationToken);
        
        return this.ToActionResult(result.Map(x => mapper.Map<GetBookmarkedJobsResponse>(x)));
    }
    
    [HttpGet]
    [Route("current/job-applications")]
    public async Task<ActionResult<GetJobApplicationsResponse>> GetJobApplications(
        [FromQuery] GetJobApplicationsRequest request,
        [FromServices] GetJobApplicationsHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetJobApplicationsQuery(request.Page, request.Size);
        
        var result = await handler.Handle(query, cancellationToken);
        
        return this.ToActionResult(result.Map(x => mapper.Map<GetJobApplicationsResponse>(x)));
    }
    
    [HttpGet]
    [Route("current/personal-files")]
    public async Task<ActionResult<GetPersonalFilesResponse>> GetPersonalFiles(
        [FromQuery] GetPersonalFilesRequest request,
        [FromServices] GetPersonalFilesHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetPersonalFilesQuery(request.Page, request.Size);
        
        var result = await handler.Handle(query, cancellationToken);
        
        return this.ToActionResult(result.Map(x => mapper.Map<GetPersonalFilesResponse>(x)));
    }
    
    [HttpGet]
    [Route("current")]
    [AllowUnconfirmedEmail]
    public async Task<ActionResult<GetUserProfileResponse>> GetUserProfile(
        [FromServices] GetUserProfileHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetUserProfileQuery();
        
        var result = await handler.Handle(query, cancellationToken);

        return this.ToActionResult(result.Map(x => mapper.Map<GetUserProfileResponse>(x)));
    }
    
    [HttpPatch]
    [Route("current")]
    public async Task<ActionResult> UpdateUserProfile(
        [FromBody] UpdateUserProfileRequest request,
        [FromServices] UpdateUserProfileHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdateUserProfileCommand(request.FirstName, request.LastName,
            request.Phone, request.IsReceivingApplicationStatusUpdates);
        
        var result = await handler.Handle(command, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpPut]
    [Route("avatar")]
    public async Task<ActionResult<UploadUserAvatarResponse>> UploadUserAvatar(
        [FromRoute] long id,
        [FromForm] IFormFile file,
        [FromServices] UploadUserAvatarHandler handler,
        CancellationToken cancellationToken)
    {
        if (file.Length == 0)
        {
            return this.ToActionResult(Result.Invalid());
        }

        await using var stream = file.OpenReadStream();
        
        var extension = Path.GetExtension(file.FileName).TrimStart('.');
        var size = file.Length;
        
        var command = new UploadUserAvatarCommand(stream, extension, size, id);
        
        var result = await handler.Handle(command, cancellationToken);

        return this.ToActionResult(result.Map(x => mapper.Map<UploadUserAvatarResponse>(x)));
    }
}
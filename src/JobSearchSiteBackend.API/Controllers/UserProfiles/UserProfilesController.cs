using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using JobSearchSiteBackend.Core.Domains._Shared.Pagination;
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
[Route("api/user")]
[Authorize]
public class UserProfilesController : ControllerBase
{
    [HttpPost]
    [Route("bookmarks/jobs")]
    public async Task<ActionResult> AddJobBookmark(
        long jobId,
        [FromServices] AddJobBookmarkHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new AddJobBookmarkRequest(jobId);
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpPost]
    public async Task<ActionResult<AddUserProfileResponse>> AddUserProfile(
        [FromBody] AddUserProfileRequest request,
        [FromServices] AddUserProfileHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpDelete]
    [Route("bookmarks/jobs")]
    public async Task<ActionResult> DeleteJobBookmark(
        long jobId,
        [FromServices] DeleteJobBookmarkHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new DeleteJobBookmarkRequest(jobId);
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpGet]
    [Route("bookmarks/jobs")]
    public async Task<ActionResult<GetBookmarkedJobsResponse>> GetBookmarkedJobs(
        [FromQuery] PaginationSpec paginationSpec,
        [FromServices] GetBookmarkedJobsHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new GetBookmarkedJobsRequest(paginationSpec);
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpGet]
    [Route("job-applications")]
    public async Task<ActionResult<GetJobApplicationsResponse>> GetJobApplications(
        [FromQuery] PaginationSpec paginationSpec,
        [FromServices] GetJobApplicationsHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new GetJobApplicationsRequest(paginationSpec);
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpGet]
    [Route("personal-files")]
    public async Task<ActionResult<GetPersonalFilesResponse>> GetPersonalFiles(
        [FromQuery] PaginationSpec paginationSpec,
        [FromServices] GetPersonalFilesHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new GetPersonalFilesRequest(paginationSpec);
        var result = await handler.Handle(request, cancellationToken);
        
        return this.ToActionResult(result);
    }
    
    [HttpGet]
    public async Task<ActionResult<GetUserProfileResponse>> GetUserProfile(
        [FromServices] GetUserProfileHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new GetUserProfileRequest();
        var result = await handler.Handle(request, cancellationToken);

        return this.ToActionResult(result);
    }
    
    [HttpPatch]
    public async Task<ActionResult> UpdateUserProfile(
        [FromBody] UpdateUserProfileRequest request,
        [FromServices] UpdateUserProfileHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
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
        
        var request = new UploadUserAvatarRequest(stream, extension, size, id);
        
        var result = await handler.Handle(request, cancellationToken);

        return this.ToActionResult(result);
    }
}
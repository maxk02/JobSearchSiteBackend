using Core.Domains._Shared.Pagination;
using Core.Domains.UserProfiles.UseCases.AddCompanyBookmark;
using Core.Domains.UserProfiles.UseCases.AddJobBookmark;
using Core.Domains.UserProfiles.UseCases.AddUserProfile;
using Core.Domains.UserProfiles.UseCases.DeleteCompanyBookmark;
using Core.Domains.UserProfiles.UseCases.DeleteJobBookmark;
using Core.Domains.UserProfiles.UseCases.GetBookmarkedCompanies;
using Core.Domains.UserProfiles.UseCases.GetBookmarkedJobs;
using Core.Domains.UserProfiles.UseCases.GetFirstCv;
using Core.Domains.UserProfiles.UseCases.GetJobApplicationsForUser;
using Core.Domains.UserProfiles.UseCases.GetPersonalFiles;
using Core.Domains.UserProfiles.UseCases.GetUserProfileById;
using Core.Domains.UserProfiles.UseCases.UpdateUserProfile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserProfilesController : ControllerBase
{
    [HttpPost]
    [Route("{userProfileId:long:min(1)}/bookmarks/companies/{companyId:long:min(1)}")]
    public async Task<ActionResult> AddCompanyBookmark(
        long userProfileId,
        long companyId,
        [FromServices] AddCompanyBookmarkHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new AddCompanyBookmarkRequest(userProfileId, companyId);
        var result = await handler.Handle(request, cancellationToken);
        
        throw new NotImplementedException();
    }
    
    [HttpPost]
    [Route("{userProfileId:long:min(1)}/bookmarks/jobs/{jobId:long:min(1)}")]
    public async Task<ActionResult> AddJobBookmark(
        long userProfileId,
        long jobId,
        [FromServices] AddJobBookmarkHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new AddJobBookmarkRequest(userProfileId, jobId);
        var result = await handler.Handle(request, cancellationToken);
        
        throw new NotImplementedException();
    }
    
    [HttpPost]
    public async Task<ActionResult> AddUserProfile(
        [FromBody] AddUserProfileRequest request,
        [FromServices] AddUserProfileHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        throw new NotImplementedException();
    }
    
    [HttpDelete]
    [Route("{userProfileId:long:min(1)}/bookmarks/companies/{companyId:long:min(1)}")]
    public async Task<ActionResult> DeleteCompanyBookmark(
        long userProfileId,
        long companyId,
        [FromServices] DeleteCompanyBookmarkHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new DeleteCompanyBookmarkRequest(userProfileId, companyId);
        var result = await handler.Handle(request, cancellationToken);
        
        throw new NotImplementedException();
    }
    
    [HttpDelete]
    [Route("{userProfileId:long:min(1)}/bookmarks/jobs/{jobId:long:min(1)}")]
    public async Task<ActionResult> DeleteJobBookmark(
        long userProfileId,
        long jobId,
        [FromServices] DeleteJobBookmarkHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new DeleteJobBookmarkRequest(userProfileId, jobId);
        var result = await handler.Handle(request, cancellationToken);
        
        throw new NotImplementedException();
    }
    
    [HttpGet]
    [Route("{id:long:min(1)}/bookmarks/companies")]
    public async Task<ActionResult> GetBookmarkedCompanies(
        long id,
        [FromQuery] PaginationSpec paginationSpec,
        [FromServices] GetBookmarkedCompaniesHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new GetBookmarkedCompaniesRequest(id, paginationSpec);
        var result = await handler.Handle(request, cancellationToken);
        
        throw new NotImplementedException();
    }
    
    [HttpGet]
    [Route("{id:long:min(1)}/bookmarks/jobs")]
    public async Task<ActionResult> GetBookmarkedJobs(
        long id,
        [FromQuery] PaginationSpec paginationSpec,
        [FromServices] GetBookmarkedJobsHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new GetBookmarkedJobsRequest(id, paginationSpec);
        var result = await handler.Handle(request, cancellationToken);
        
        throw new NotImplementedException();
    }
    
    [HttpGet]
    [Route("{id:long:min(1)}/cv")]
    public async Task<ActionResult> GetFirstCv(
        long id,
        [FromServices] GetFirstCvHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new GetFirstCvRequest(id);
        var result = await handler.Handle(request, cancellationToken);
        
        throw new NotImplementedException();
    }
    
    [HttpGet]
    [Route("{id:long:min(1)}/job-applications")]
    public async Task<ActionResult> GetJobApplications(
        long id,
        [FromQuery] PaginationSpec paginationSpec,
        [FromServices] GetJobApplicationsForUserHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new GetJobApplicationsForUserRequest(id, paginationSpec);
        var result = await handler.Handle(request, cancellationToken);
        
        throw new NotImplementedException();
    }
    
    [HttpGet]
    [Route("{id:long:min(1)}/personal-files")]
    public async Task<ActionResult> GetPersonalFiles(
        long id,
        [FromQuery] PaginationSpec paginationSpec,
        [FromServices] GetPersonalFilesHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new GetPersonalFilesRequest(id, paginationSpec);
        var result = await handler.Handle(request, cancellationToken);
        
        throw new NotImplementedException();
    }
    
    [HttpGet("{id:long:min(1)}")]
    public async Task<ActionResult<GetUserProfileByIdResponse>> GetUserProfile(
        long id, 
        [FromServices] GetUserProfileByIdHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new GetUserProfileByIdRequest(id);
        var response = await handler.Handle(request, cancellationToken);

        throw new NotImplementedException();
    }
    
    [HttpPatch]
    public async Task<ActionResult> UpdateUserProfile(
        [FromBody] UpdateUserProfileRequest request,
        [FromServices] UpdateUserProfileHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        
        throw new NotImplementedException();
    }
}
using Core.Domains.UserProfiles;
using Core.Domains.UserProfiles.UseCases.GetUserProfileById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/user-profiles")]
public class UserProfileController(IUserProfileRepository userProfileRepository) : ControllerBase
{
    [HttpGet("{Id}")]
    [Authorize]
    public async Task<ActionResult<GetUserProfileByIdResponse>> GetUserProfileById(GetUserProfileByIdRequest request, 
        CancellationToken cancellationToken = default)
    {
        // var result = await new GetUserProfileByIdHandler().Handle(request, cancellationToken);
        throw new NotImplementedException();
    }
}
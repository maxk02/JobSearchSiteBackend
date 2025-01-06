using Core.Domains.UserProfiles;
using Core.Domains.UserProfiles.UseCases.GetUserProfileById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/user-profiles")]
public class UserProfileController : ControllerBase
{
    [HttpGet("{id:long:min(1)}")]
    [Authorize]
    public async Task<ActionResult<GetUserProfileByIdResponse>> GetUserProfileById(long id, 
        CancellationToken cancellationToken = default)
    {
        // var result = await new GetUserProfileByIdHandler().Handle(request, cancellationToken);
        throw new NotImplementedException();
    }
}
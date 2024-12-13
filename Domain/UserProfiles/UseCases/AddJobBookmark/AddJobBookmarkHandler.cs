using Domain._Shared.Services.Auth;
using Shared.Result;

namespace Domain.UserProfiles.UseCases.AddJobBookmark;

public class AddJobBookmarkHandler(ICurrentAccountService currentAccountService, IUserProfileRepository userProfileRepository)
{
    public async Task<Result> Handle(AddJobBookmarkRequest request, CancellationToken cancellationToken = default)
    {
        var currentAccountId = currentAccountService.GetId();

        if (currentAccountId is null)
            return Result.Unauthorized();
        
        if (currentAccountId != request.UserId)
            return Result.Forbidden();
        
        await userProfileRepository.AddJobBookmarkAsync(request.UserId, request.JobId, cancellationToken);

        return Result.Success();
    }
}
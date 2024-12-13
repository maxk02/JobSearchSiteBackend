using Domain._Shared.Services.Auth;
using Shared.Result;

namespace Domain.UserProfiles.UseCases.RemoveJobBookmark;

public class RemoveJobBookmarkHandler(ICurrentAccountService currentAccountService, IUserProfileRepository userProfileRepository)
{
    public async Task<Result> Handle(RemoveJobBookmarkRequest request, CancellationToken cancellationToken = default)
    {
        var currentAccountId = currentAccountService.GetId();

        if (currentAccountId is null)
            return Result.Unauthorized();
        
        if (currentAccountId != request.UserId)
            return Result.Forbidden();
        
        await userProfileRepository.RemoveJobBookmarkAsync(request.UserId, request.JobId, cancellationToken);

        return Result.Success();
    }
}
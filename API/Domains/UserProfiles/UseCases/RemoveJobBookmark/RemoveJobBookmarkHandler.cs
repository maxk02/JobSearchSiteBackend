using API.Services.Auth;
using API.Services.Auth.CurrentAccount;
using Shared.Result;

namespace API.Domains.UserProfiles.UseCases.RemoveJobBookmark;

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
using Core.Domains._Shared.UseCaseStructure;
using Core.Services.Auth.Authentication;
using Shared.Result;

namespace Core.Domains.UserProfiles.UseCases.RemoveJobBookmark;

public class RemoveJobBookmarkHandler(ICurrentAccountService currentAccountService,
    IUserProfileRepository userProfileRepository) : IRequestHandler<RemoveJobBookmarkRequest, Result>
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
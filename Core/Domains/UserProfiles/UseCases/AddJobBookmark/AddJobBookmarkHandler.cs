using Core.Domains._Shared.UseCaseStructure;
using Core.Services.Auth.Authentication;
using Shared.Result;

namespace Core.Domains.UserProfiles.UseCases.AddJobBookmark;

public class AddJobBookmarkHandler(ICurrentAccountService currentAccountService,
    IUserProfileRepository userProfileRepository) : IRequestHandler<AddJobBookmarkRequest, Result>
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
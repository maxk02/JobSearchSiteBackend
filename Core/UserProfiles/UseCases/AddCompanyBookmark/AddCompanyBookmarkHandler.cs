using Core._Shared.Services.Auth;
using Shared.Result;

namespace Core.UserProfiles.UseCases.AddCompanyBookmark;

public class AddCompanyBookmarkHandler(IUserProfileRepository userProfileRepository, ICurrentAccountService currentAccountService)
{
    public async Task<Result> Handle(AddCompanyBookmarkRequest request, CancellationToken cancellationToken = default)
    {
        var currentAccountId = currentAccountService.GetId();

        if (currentAccountId is null)
            return Result.Unauthorized();
        
        if (currentAccountId != request.UserId)
            return Result.Forbidden();
        
        await userProfileRepository.AddCompanyBookmarkAsync(request.UserId, request.CompanyId, cancellationToken);

        return Result.Success();
    }
}
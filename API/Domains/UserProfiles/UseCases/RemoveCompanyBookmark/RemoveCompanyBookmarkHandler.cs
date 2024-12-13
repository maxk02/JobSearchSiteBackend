using API.Services.Auth;
using API.Services.Auth.CurrentAccount;
using Shared.Result;

namespace API.Domains.UserProfiles.UseCases.RemoveCompanyBookmark;

public class RemoveCompanyBookmarkHandler(IUserProfileRepository userProfileRepository, ICurrentAccountService currentAccountService)
{
    public async Task<Result> Handle(RemoveCompanyBookmarkRequest request, CancellationToken cancellationToken = default)
    {
        var currentAccountId = currentAccountService.GetId();

        if (currentAccountId is null)
            return Result.Unauthorized();
        
        if (currentAccountId != request.UserId)
            return Result.Forbidden();
        
        await userProfileRepository.RemoveCompanyBookmarkAsync(request.UserId, request.CompanyId, cancellationToken);

        return Result.Success();
    }
}
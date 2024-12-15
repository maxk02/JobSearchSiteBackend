using Core.Domains.UserProfiles;
using Core.Services.Auth.AccountStorage;
using Core.Services.Auth.Authentication;
using Shared.Result;

namespace Core.Domains.Accounts.DeleteAccount;

public class DeleteAccountHandler(IAccountService accountService, IUserProfileRepository userProfileRepository,
    ICurrentAccountService currentAccountService)
{
    public async Task<Result> Handle(DeleteAccountRequest request, CancellationToken cancellationToken = default)
    {
        var currentAccountId = currentAccountService.GetId();

        if (currentAccountId is null)
            return Result.Unauthorized();
        
        var deletionResult = await accountService.DeleteAsync(request.Id, cancellationToken);
        
        if (!deletionResult.IsSuccess)
        {
            return deletionResult;
        }
        
        var userToRemove = await userProfileRepository.GetByIdAsync(request.Id, CancellationToken.None);
        if (userToRemove is not null)
        {
            await userProfileRepository.RemoveAsync(userToRemove, CancellationToken.None);
        }

        return deletionResult;
    }
}
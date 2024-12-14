using Core._Shared.Services.Auth;
using Core.UserProfiles;
using Shared.Result;

namespace Core._Account.DeleteAccount;

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
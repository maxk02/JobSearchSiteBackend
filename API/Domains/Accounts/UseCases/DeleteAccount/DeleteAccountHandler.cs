using API.Domains.UserProfiles;
using API.Services.Auth.AccountStorage;
using API.Services.Auth.CurrentAccount;
using Shared.Result;

namespace API.Domains.Accounts.UseCases.DeleteAccount;

public class DeleteAccountHandler(IAccountStorageService accountStorageService, IUserProfileRepository userProfileRepository,
    ICurrentAccountService currentAccountService)
{
    public async Task<Result> Handle(DeleteAccountRequest request, CancellationToken cancellationToken = default)
    {
        var currentAccountId = currentAccountService.GetId();

        if (currentAccountId is null)
            return Result.Unauthorized();
        
        var deletionResult = await accountStorageService.DeleteAsync(request.Id, cancellationToken);
        
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
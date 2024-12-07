using Domain._Shared.Services.Auth;
using Domain.Users;
using Shared.Result;

namespace Domain._Account.DeleteAccount;

public class DeleteAccountHandler(IAccountService accountService, IUserRepository userRepository)
{
    public async Task<Result> Handle(DeleteAccountRequest request, CancellationToken cancellationToken = default)
    {
        var deletionResult = await accountService.DeleteAsync(request.Id, cancellationToken);
        
        if (!deletionResult.IsSuccess)
        {
            return deletionResult;
        }
        
        await userRepository.RemoveByAccountIdAsync(request.Id, CancellationToken.None);

        return deletionResult;
    }
}
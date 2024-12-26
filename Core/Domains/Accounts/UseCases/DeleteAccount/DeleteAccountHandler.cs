using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.UserProfiles;
using Core.Services.Auth.AccountStorage;
using Core.Services.Auth.Authentication;
using Shared.Result;

namespace Core.Domains.Accounts.UseCases.DeleteAccount;

public class DeleteAccountHandler(ICurrentAccountService currentAccountService,
    IAccountStorageService accountStorageService,
    IUserProfileRepository userProfileRepository) : IRequestHandler<DeleteAccountRequest, Result>
{
    public async Task<Result> Handle(DeleteAccountRequest request, CancellationToken cancellationToken = default)
    {
        if (currentAccountService.GetIdOrThrow() != request.Id)
            return Result.Forbidden();
        
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
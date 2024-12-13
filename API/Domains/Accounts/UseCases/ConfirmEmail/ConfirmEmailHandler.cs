using API.Services.Auth.AccountStorage;
using Shared.Result;

namespace API.Domains.Accounts.UseCases.ConfirmEmail;

public class ConfirmEmailHandler(IAccountStorageService accountStorageService)
{
    public async Task<Result> Handle(ConfirmEmailRequest request, CancellationToken cancellationToken)
    {
        var confirmEmailResult = await accountStorageService
            .ConfirmEmailAsync(request.Token, cancellationToken);
        
        return confirmEmailResult;
    }
}
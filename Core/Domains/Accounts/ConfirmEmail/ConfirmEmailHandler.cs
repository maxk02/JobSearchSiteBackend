using Core.Services.Auth.AccountStorage;
using Shared.Result;

namespace Core.Domains.Accounts.ConfirmEmail;

public class ConfirmEmailHandler(IAccountService accountService)
{
    public async Task<Result> Handle(ConfirmEmailRequest request, CancellationToken cancellationToken)
    {
        var confirmEmailResult = await accountService
            .ConfirmEmailAsync(request.Token, cancellationToken);
        
        return confirmEmailResult;
    }
}
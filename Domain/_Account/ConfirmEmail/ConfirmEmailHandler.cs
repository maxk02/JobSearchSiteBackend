using Domain._Shared.Services.Auth;
using Shared.Result;

namespace Domain._Account.ConfirmEmail;

public class ConfirmEmailHandler(IAccountService accountService)
{
    public async Task<Result> Handle(ConfirmEmailRequest request, CancellationToken cancellationToken)
    {
        var confirmEmailResult = await accountService
            .ConfirmEmailAsync(request.Token, cancellationToken);
        
        return confirmEmailResult;
    }
}
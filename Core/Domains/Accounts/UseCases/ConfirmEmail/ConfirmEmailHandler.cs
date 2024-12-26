using Core.Domains._Shared.UseCaseStructure;
using Core.Services.Auth.AccountStorage;
using Shared.Result;

namespace Core.Domains.Accounts.UseCases.ConfirmEmail;

public class ConfirmEmailHandler(IAccountStorageService accountStorageService) 
    : IRequestHandler<ConfirmEmailRequest, Result>
{
    public async Task<Result> Handle(ConfirmEmailRequest request, CancellationToken cancellationToken = default)
    {
        var confirmEmailResult = await accountStorageService
            .ConfirmEmailAsync(request.Token, cancellationToken);
        
        return confirmEmailResult;
    }
}
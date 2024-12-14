using Core._Shared.Services.Auth;
using Shared.Result;

namespace Core._Account.CreateAccount;

public class CreateAccountHandler(IAccountService accountService,
    ICurrentAccountService currentAccountService,
    IAccountTokenGenerationService accountTokenGenerationService)
{
    public async Task<Result<CreateAccountResponse>> Handle(CreateAccountRequest request,
        CancellationToken cancellationToken = default)
    {
        var accServiceCreationResult = await accountService
            .RegisterAsync(request.Email, request.Password, cancellationToken);
        
        if (accServiceCreationResult.Value is null)
            return Result<CreateAccountResponse>.WithMetadataFrom(accServiceCreationResult);

        var token = accountTokenGenerationService.Generate(accServiceCreationResult.Value);

        return new CreateAccountResponse(token);
    }
}
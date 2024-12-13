using API.Services.Auth.AccountStorage;
using API.Services.Auth.CurrentAccount;
using Shared.Result;

namespace API.Domains.Accounts.UseCases.CreateAccount;

public class CreateAccountHandler(IAccountStorageService accountStorageService,
    ICurrentAccountService currentAccountService,
    IAccountTokenGenerationService accountTokenGenerationService)
{
    public async Task<Result<CreateAccountResponse>> Handle(CreateAccountRequest request,
        CancellationToken cancellationToken = default)
    {
        var accServiceCreationResult = await accountStorageService
            .RegisterAsync(request.Email, request.Password, cancellationToken);
        
        if (accServiceCreationResult.Value is null)
            return Result<CreateAccountResponse>.WithMetadataFrom(accServiceCreationResult);

        var token = accountTokenGenerationService.Generate(accServiceCreationResult.Value);

        return new CreateAccountResponse(token);
    }
}
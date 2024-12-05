using Domain._Shared.Services.Auth;
using Shared.Result;

namespace Domain._Account.CreateAccount;

public class CreateAccountHandler(IAccountService accountService,
    ICurrentAccountService currentAccountService,
    ITokenGenerationService tokenGenerationService)
{
    public async Task<Result<CreateAccountResponse>> Handle(CreateAccountRequest request,
        CancellationToken cancellationToken = default)
    {
        var creationResult = await accountService.RegisterAsync(request.Email, request.Password);
        
        if (creationResult.Value is null)
            return Result.WithMetadataFromResult(creationResult);

        var token = tokenGenerationService.Generate(creationResult.Value);

        return new CreateAccountResponse(token);
    }
}
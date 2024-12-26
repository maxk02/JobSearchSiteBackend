using Core.Domains._Shared.UseCaseStructure;
using Core.Services.Auth.AccountStorage;
using Core.Services.Auth.Authentication;
using Shared.Result;

namespace Core.Domains.Accounts.UseCases.CreateAccount;

public class CreateAccountHandler(IAccountStorageService accountStorageService,
    IAccountTokenGenerationService accountTokenGenerationService) 
    : IRequestHandler<CreateAccountRequest, Result<CreateAccountResponse>>
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
using Core.Domains._Shared.UseCaseStructure;
using Core.Services.Auth.AccountStorage;
using Core.Services.Auth.Authentication;
using Shared.Result;

namespace Core.Domains.Accounts.UseCases.CreateAccount;

public class CreateAccountHandler(IIdentityService identityService,
    IJwtGenerationService jwtGenerationService) 
    : IRequestHandler<CreateAccountRequest, Result<CreateAccountResponse>>
{
    public async Task<Result<CreateAccountResponse>> Handle(CreateAccountRequest request,
        CancellationToken cancellationToken = default)
    {
        var accServiceCreationResult = await identityService
            .RegisterAsync(request.Email, request.Password, cancellationToken);
        
        if (accServiceCreationResult.Value is null)
            return Result<CreateAccountResponse>.WithMetadataFrom(accServiceCreationResult);

        var token = jwtGenerationService.Generate(accServiceCreationResult.Value);

        return new CreateAccountResponse(token);
    }
}
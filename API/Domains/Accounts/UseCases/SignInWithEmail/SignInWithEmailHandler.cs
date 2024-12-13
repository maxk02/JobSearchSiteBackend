using API.Services.Auth.AccountStorage;
using API.Services.Auth.CurrentAccount;
using Shared.Result;

namespace API.Domains.Accounts.UseCases.SignInWithEmail;

public class SignInWithEmailHandler(IAccountStorageService accountStorageService,
    IAccountTokenGenerationService accountTokenGenerationService)
{
    public async Task<Result<SignInWithEmailResponse>> Handle(SignInWithEmailRequest request,
        CancellationToken cancellationToken = default)
    {
        var matchedAccountDataResult = await accountStorageService
            .SignInWithEmailAsync(request.Email, request.Password, cancellationToken);

        if (matchedAccountDataResult.Value is null)
            return Result<SignInWithEmailResponse>.WithMetadataFrom(matchedAccountDataResult);
        
        var token = accountTokenGenerationService.Generate(matchedAccountDataResult.Value);

        return new SignInWithEmailResponse(token);
    }
}
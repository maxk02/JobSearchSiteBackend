using Domain._Shared.Services.Auth;
using Shared.Result;

namespace Domain._Account.SignInWithEmail;

public class SignInWithEmailHandler(IAccountService accountService,
    IAccountTokenGenerationService accountTokenGenerationService)
{
    public async Task<Result<SignInWithEmailResponse>> Handle(SignInWithEmailRequest request,
        CancellationToken cancellationToken = default)
    {
        var matchedAccountDataResult = await accountService
            .SignInWithEmailAsync(request.Email, request.Password, cancellationToken);

        if (matchedAccountDataResult.Value is null)
            return Result<SignInWithEmailResponse>.WithMetadataFrom(matchedAccountDataResult);
        
        var token = accountTokenGenerationService.Generate(matchedAccountDataResult.Value);

        return new SignInWithEmailResponse(token);
    }
}
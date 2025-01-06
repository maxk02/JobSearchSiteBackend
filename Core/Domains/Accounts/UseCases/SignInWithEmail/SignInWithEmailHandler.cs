using Core.Domains._Shared.UseCaseStructure;
using Core.Services.Auth.AccountStorage;
using Core.Services.Auth.Authentication;
using Shared.Result;

namespace Core.Domains.Accounts.UseCases.SignInWithEmail;

public class SignInWithEmailHandler(IIdentityService identityService,
    IJwtGenerationService jwtGenerationService) 
    : IRequestHandler<SignInWithEmailRequest, Result<SignInWithEmailResponse>>
{
    public async Task<Result<SignInWithEmailResponse>> Handle(SignInWithEmailRequest request, CancellationToken cancellationToken = default)
    {
        var matchedAccountDataResult = await identityService
            .SignInWithEmailAsync(request.Email, request.Password, cancellationToken);

        if (matchedAccountDataResult.Value is null)
            return Result<SignInWithEmailResponse>.WithMetadataFrom(matchedAccountDataResult);
        
        var token = jwtGenerationService.Generate(matchedAccountDataResult.Value);

        return new SignInWithEmailResponse(token);
    }
}
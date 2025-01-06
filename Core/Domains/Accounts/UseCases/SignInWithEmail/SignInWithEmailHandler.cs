using Core.Domains._Shared.UseCaseStructure;
using Core.Persistence.EfCore.AspNetCoreIdentity;
using Core.Services.Auth;
using Microsoft.AspNetCore.Identity;
using Shared.Result;

namespace Core.Domains.Accounts.UseCases.SignInWithEmail;

public class SignInWithEmailHandler(UserManager<MyIdentityUser> userManager,
    IJwtGenerationService jwtGenerationService) 
    : IRequestHandler<SignInWithEmailRequest, Result<SignInWithEmailResponse>>
{
    public async Task<Result<SignInWithEmailResponse>> Handle(SignInWithEmailRequest request, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null)
            return Result<SignInWithEmailResponse>.NotFound();

        var isPasswordCorrect = await userManager.CheckPasswordAsync(user, request.Password);

        if (!isPasswordCorrect)
            return Result<SignInWithEmailResponse>.Unauthorized();

        var roles = await userManager.GetRolesAsync(user);

        var accountData = new AccountData(user.Id, request.Email, roles);
        
        var token = jwtGenerationService.Generate(accountData);

        return new SignInWithEmailResponse(token);
    }
}
using Core.Domains._Shared.UseCaseStructure;
using Core.Persistence.EfCore.AspNetCoreIdentity;
using Core.Services.Auth;
using Microsoft.AspNetCore.Identity;
using Shared.Result;

namespace Core.Domains.Accounts.UseCases.CreateAccount;

public class CreateAccountHandler(UserManager<MyIdentityUser> userManager,
    IJwtGenerationService jwtGenerationService) 
    : IRequestHandler<CreateAccountRequest, Result<CreateAccountResponse>>
{
    public async Task<Result<CreateAccountResponse>> Handle(CreateAccountRequest request,
        CancellationToken cancellationToken = default)
    {
        var userFromDb = await userManager.FindByEmailAsync(request.Email);

        if (userFromDb is not null)
            return Result<CreateAccountResponse>.Conflict("User already registered.");

        var user = new MyIdentityUser { Email = request.Email };
        var aspNetIdentityResult = await userManager.CreateAsync(user, request.Password);

        if (!aspNetIdentityResult.Succeeded)
            return Result<CreateAccountResponse>.Error();

        var accountData = new AccountData(user.Id, user.Email, []);

        var token = jwtGenerationService.Generate(accountData);

        return new CreateAccountResponse(token);
    }
}
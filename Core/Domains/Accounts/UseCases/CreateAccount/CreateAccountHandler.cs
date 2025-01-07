using Core.Domains._Shared.UseCaseStructure;
using Core.Persistence.EfCore;
using Core.Persistence.EfCore.EntityConfigs.AspNetCoreIdentity;
using Core.Services.Auth;
using Microsoft.AspNetCore.Identity;
using Shared.Result;

namespace Core.Domains.Accounts.UseCases.CreateAccount;

public class CreateAccountHandler(UserManager<MyIdentityUser> userManager,
    MainDataContext context,
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

        var accountData = new AccountData(user.Id, []);

        var newTokenId = Guid.NewGuid();
        
        var token = jwtGenerationService.Generate(accountData, newTokenId);

        var userSessionCreationResult = UserSession.Create(newTokenId.ToString(), user.Id, DateTime.UtcNow,
            DateTime.UtcNow.Add(TimeSpan.FromDays(30)), request.Device, request.Os, request.Client);
        
        if (userSessionCreationResult.IsFailure)
            return Result<CreateAccountResponse>.Error();

        var newUserSession = userSessionCreationResult.Value;
        
        context.UserSessions.Add(newUserSession);
        await context.SaveChangesAsync(cancellationToken);

        return new CreateAccountResponse(token);
    }
}
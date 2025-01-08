using Core.Domains._Shared.UseCaseStructure;
using Core.Persistence.EfCore;
using Core.Persistence.EfCore.EntityConfigs.AspNetCoreIdentity;
using Core.Services.Auth;
using Microsoft.AspNetCore.Identity;
using Shared.Result;

namespace Core.Domains.Accounts.UseCases.SignInWithEmail;

public class SignInWithEmailHandler(UserManager<MyIdentityUser> userManager,
    MainDataContext context,
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

        var accountData = new AccountData(user.Id, roles);
        
        var newTokenId = Guid.NewGuid();
        
        var token = jwtGenerationService.Generate(accountData, newTokenId);
        
        var newUserSession = new UserSession(newTokenId.ToString(), user.Id, DateTime.UtcNow,
            DateTime.UtcNow.Add(TimeSpan.FromDays(30)), request.Device, request.Os, request.Client);
        
        context.UserSessions.Add(newUserSession);
        await context.SaveChangesAsync(cancellationToken);

        return new SignInWithEmailResponse(token);
    }
}
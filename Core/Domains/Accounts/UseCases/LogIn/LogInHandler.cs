using Core.Domains._Shared.UseCaseStructure;
using Core.Persistence.EfCore;
using Core.Persistence.EfCore.EntityConfigs.AspNetCoreIdentity;
using Core.Services.Auth;
using Microsoft.AspNetCore.Identity;
using Shared.Result;

namespace Core.Domains.Accounts.UseCases.LogIn;

public class LogInHandler(UserManager<MyIdentityUser> userManager,
    MainDataContext context,
    IJwtGenerationService jwtGenerationService) 
    : IRequestHandler<LogInRequest, Result<LogInResponse>>
{
    public async Task<Result<LogInResponse>> Handle(LogInRequest request, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null)
            return Result<LogInResponse>.NotFound();

        var isPasswordCorrect = await userManager.CheckPasswordAsync(user, request.Password);

        if (!isPasswordCorrect)
            return Result<LogInResponse>.Unauthorized();

        var roles = await userManager.GetRolesAsync(user);

        var accountData = new AccountData(user.Id, roles);
        
        var newTokenId = Guid.NewGuid();
        
        var token = jwtGenerationService.Generate(accountData, newTokenId);
        
        var newUserSession = new UserSession(newTokenId.ToString(), user.Id, DateTime.UtcNow,
            DateTime.UtcNow.Add(TimeSpan.FromDays(30)), request.Device, request.Os, request.Client);
        
        context.UserSessions.Add(newUserSession);
        await context.SaveChangesAsync(cancellationToken);

        return new LogInResponse(token);
    }
}
using Core.Domains._Shared.UseCaseStructure;
using Core.Persistence.EfCore;
using Core.Services.Auth;
using Shared.Result;

namespace Core.Domains.Accounts.UseCases.SignOut;

public class SignOutHandler(IJwtCurrentAccountService jwtCurrentAccountService,
    MainDataContext context) : IRequestHandler<SignOutRequest, Result>
{
    public async Task<Result> Handle(SignOutRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserJwtId = jwtCurrentAccountService.GetTokenIdentifierOrThrow();
        
        var newBlacklistedJwt = new BlacklistedJwt(currentUserJwtId);
        
        context.BlacklistedJwts.Add(newBlacklistedJwt);
        
        await context.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}
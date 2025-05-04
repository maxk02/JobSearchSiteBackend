using Core.Domains._Shared.UseCaseStructure;
using Core.Services.Auth;
using Ardalis.Result;
using Core.Persistence;

namespace Core.Domains.Accounts.UseCases.ExtendSession;

public class ExtendSessionHandler(ICurrentAccountService currentAccountService,
    MainDataContext context) : IRequestHandler<ExtendSessionRequest, Result<ExtendSessionResponse>>
{
    public async Task<Result<ExtendSessionResponse>> Handle(ExtendSessionRequest request, CancellationToken cancellationToken = default)
    {
        var tokenId = currentAccountService.GetTokenIdentifierOrThrow();
        var userId = currentAccountService.GetIdOrThrow();

        var session = await context.UserSessions.FindAsync([tokenId], cancellationToken);
        
        if (session is null || session.UserId != userId || DateTime.UtcNow > session.ExpiresUtc)
        {
            throw new UnauthorizedAccessException();
        }
        
        var newExpirationTimeUtc = session.ExpiresUtc.AddDays(30);
        
        var newUserSession = new UserSession(session.TokenId, session.UserId,
            session.FirstTimeIssuedUtc, newExpirationTimeUtc);
        
        context.UserSessions.Add(newUserSession);
        await context.SaveChangesAsync(cancellationToken);

        var response = new ExtendSessionResponse(newExpirationTimeUtc);

        return Result<ExtendSessionResponse>.Success(response);
    }
}
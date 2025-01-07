using Core.Domains.UserProfiles;
using Shared.Result;
using Shared.Result.FluentValidation;

namespace Core.Domains.Accounts;

public class UserSession
{
    public static UserSessionValidator Validator { get; } = new();

    public static Result<UserSession> Create(string tokenId, long userId, DateTime firstTimeIssuedUtc,
        DateTime expiresUtc, string? lastDevice, string? lastOs, string? lastClient)
    {
        var userSession =
            new UserSession(tokenId, userId, firstTimeIssuedUtc, expiresUtc, lastDevice, lastOs, lastClient);
        
        var validationResult = Validator.Validate(userSession);
        
        return validationResult.IsValid ? userSession : Result<UserSession>.Invalid(validationResult.AsErrors());
    }

    private UserSession(string tokenId, long userId, DateTime firstTimeIssuedUtc, DateTime expiresUtc,
        string? lastDevice, string? lastOs, string? lastClient)
    {
        TokenId = tokenId;
        UserId = userId;
        FirstTimeIssuedUtc = firstTimeIssuedUtc;
        ExpiresUtc = expiresUtc;
        LastDevice = lastDevice;
        LastOs = lastOs;
        LastClient = lastClient;
    }

    public string TokenId { get; private set; }

    public long UserId { get; private set; }

    public DateTime FirstTimeIssuedUtc { get; private set; }

    public DateTime ExpiresUtc { get; private set; }

    public string? LastDevice { get; private set; }

    public string? LastOs { get; private set; }

    public string? LastClient { get; private set; }

    public UserProfile? UserProfile { get; private set; }
}
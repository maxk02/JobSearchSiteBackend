using Core.Domains.UserProfiles;
using Ardalis.Result;
using Ardalis.Result.FluentValidation;

namespace Core.Domains.Accounts;

public class UserSession
{
    public UserSession(string token, long userId, DateTime firstTimeIssuedUtc, DateTime expiresUtc)
    {
        Token = token;
        UserId = userId;
        FirstTimeIssuedUtc = firstTimeIssuedUtc;
        ExpiresUtc = expiresUtc;
    }

    public string Token { get; private set; }

    public long UserId { get; private set; }

    public DateTime FirstTimeIssuedUtc { get; private set; }

    public DateTime ExpiresUtc { get; private set; }

    public UserProfile? UserProfile { get; private set; }
}
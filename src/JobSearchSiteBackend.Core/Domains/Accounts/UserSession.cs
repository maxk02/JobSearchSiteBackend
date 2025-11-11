// using JobSearchSiteBackend.Core.Domains.UserProfiles;
// using Ardalis.Result;
// using Ardalis.Result.FluentValidation;
//
// namespace JobSearchSiteBackend.Core.Domains.Accounts;
//
// public class UserSession
// {
//     public UserSession(string tokenId, long userId, DateTime firstTimeIssuedUtc, DateTime expiresUtc)
//     {
//         TokenId = tokenId;
//         UserId = userId;
//         FirstTimeIssuedUtc = firstTimeIssuedUtc;
//         ExpiresUtc = expiresUtc;
//     }
//
//     public string TokenId { get; private set; }
//
//     public long UserId { get; private set; }
//
//     public DateTime FirstTimeIssuedUtc { get; private set; }
//
//     public DateTime ExpiresUtc { get; private set; }
//
//     public UserProfile? UserProfile { get; private set; }
// }
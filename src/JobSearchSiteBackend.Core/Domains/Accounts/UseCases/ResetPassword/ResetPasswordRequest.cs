using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.Accounts.UseCases.ResetPassword;

public record ResetPasswordRequest(string Token, string NewPassword) : IRequest<Result>;
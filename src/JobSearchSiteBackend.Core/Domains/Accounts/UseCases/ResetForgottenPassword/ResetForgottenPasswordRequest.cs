using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.Accounts.UseCases.ResetForgottenPassword;

public record ResetForgottenPasswordRequest(string Token, string NewPassword) : IRequest<Result>;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.Accounts.UseCases.LogIn;

public record LogInRequest(
    string Email,
    string Password) : IRequest<Result<LogInResponse>>;
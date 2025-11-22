using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.Accounts.UseCases.CreateAccount;

public record CreateAccountCommand(string Email, string Password) : IRequest<Result>;
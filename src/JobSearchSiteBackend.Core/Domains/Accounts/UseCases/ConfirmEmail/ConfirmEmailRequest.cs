using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.Accounts.UseCases.ConfirmEmail;

public record ConfirmEmailRequest(string Token) : IRequest<Result>;
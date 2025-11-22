using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.Accounts.UseCases.SendPasswordResetLink;

public record SendPasswordResetLinkCommand(string Email) : IRequest<Result>;
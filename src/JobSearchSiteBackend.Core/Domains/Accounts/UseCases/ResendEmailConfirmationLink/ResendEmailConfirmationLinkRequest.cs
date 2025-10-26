using Ardalis.Result;
using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.Accounts.UseCases.ResendEmailConfirmationLink;

public record ResendEmailConfirmationLinkRequest : IRequest<Result>;
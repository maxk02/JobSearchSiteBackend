using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.Accounts.UseCases.SendEmailConfirmationLink;

public record SendEmailConfirmationLinkRequest : IRequest<Result>;
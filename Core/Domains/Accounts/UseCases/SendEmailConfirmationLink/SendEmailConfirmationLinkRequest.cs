using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.Accounts.UseCases.SendEmailConfirmationLink;

public record SendEmailConfirmationLinkRequest(string Email) : IRequest<Result>;
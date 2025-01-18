using Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace Core.Domains.Accounts.UseCases.SendEmailConfirmationLink;

public record SendEmailConfirmationLinkRequest(string Email) : IRequest<Result>;
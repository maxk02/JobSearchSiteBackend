using Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace Core.Domains.Accounts.UseCases.SendPasswordResetLink;

public record SendPasswordResetLinkRequest(string Email) : IRequest<Result>;
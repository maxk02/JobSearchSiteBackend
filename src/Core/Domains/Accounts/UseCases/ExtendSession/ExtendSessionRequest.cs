using Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace Core.Domains.Accounts.UseCases.ExtendSession;

public record ExtendSessionRequest : IRequest<Result<ExtendSessionResponse>>;